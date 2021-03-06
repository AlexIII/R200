# R200 Relay Computer
<img width="600" src="img/titleImg.jpg" />

*oh gods, here goes nothing…*

## About
R200 project is a personal project centered around creating general purpose relay computer. "200" in the name suggests that the machine will be constructed with use of about 200 electromagnetic relays.

**Scroll to the end of this file for Current construction progress, Photo and Video.**

## but WHY?!
I mean... You've probably come to the wrong place if that's the question that bothers you. (= Seriously, though, the purpose of the project is entirely recreational. i.e. DON'T FORGET YOU'RE HERE FOR FUN.

With that being said, let's get down to business.

## Ground rules
1.	The machine should be self-sufficient and independent of any external entities except for power supply and a human operator.
2.	No semiconductors allowed except diodes. (there’s gonna be TONS of diodes, though)

## Machine specs
The machine is based on the harvard architecture (separate program and data memory).
### Block diagram
<img width="600" src="img/block-dia.png" />

### Specs
Bus
- 12-bit data bus
- 8-bit instructions
- 6-bit program address bus
- 4-bit RAM address bus
- 4-bit CONST memory address bus

Memory
- ROM (Program memory): 64 x 8-bit instructions (implemented as array of 64 by 8 dip-switches)
- CONST MEMORY: 16 x 12-bit words (implemented as array of 16 by 12 dip-switches)
- RAM: 8 x 12-bit words (implemented as array of 8 by 12 electrolytic capacitors)
- Registers: 
  - 2x12-bit GPRs [RA and RB] (implemented as array of 2 by 12 relays)
  - 6-bit Program Counter [PC] (implemented as array of 6 relays)
  - 6-bit Leaf Register (implemented as array of 6 electrolytic capacitors)

ALU
- Some of 16 functions of ALU: add/sub, inc/dec, shift right/left, bitwise not/and/or/xor
- ALU can only operates on GPRs [RA and RB]
- Carry Flag [CF]
- Zero Flag [ZF]

I/O
- Input: dip-switches of CONST MEMORY are designed to be used as user input
- Output: every bit of GPRs [RA and RB] has an LED. At the end of a program the result should be loaded in GPRs in order to be read by user

Clock
- Every instruction is executed in one machine cycle. All instructions are single-worded. One machine cycle consists of 2 clock pulses. One machine cycle takes approximately 0.45sec. of real time.

Hardware
- 200+ 48V 4PDT (4-pole double-throw) electromagnetic relays 
- 1000+ 1N4001 diodes
- 150+ 220uF electrolytic capacitors
- 40+ LEDs
- 704 individual switches
- 300+ resistors
- 3A power supply (temporary switched-mode type, so technically it breaks the second ground rule)

## Documentation
Somewhat finished pieces:
- <a href="doc/R200 Instruction Set.pdf">R200 Instruction Set</a> [Note: for ALU instructions if Rd is RA, then Rs is RB and vice-versa.]
- <a href="schematics/R200-AUG-2018.pdf">Schematic of currently assembled part</a> (Program Counter, ROM and CLOCK GENERATOR / SEQUENCER)



## Important notes
Due to the physical implementation limitations, any RAM word and LEAF register can only be read ones. Once the RAM location or LEAF register has been read, its state becomes undefined. That’s why if you want to preserve value in RAM, you must write it back to the same location  every time it's read. In the emulator RAM word resets to 0 after it's read.

## Emulation 
There’s simple command-line emulator available, made in C#. It accepts assembler file as an input, does simple preprocessing and executes the programm. No actual byte-code generation present.

**Update**: Thanks to <a href="https://github.com/atsidaev">Alexander Tsidaev</a> there's now GUI that visualizes the machine memory!

Here's the emulator running `fibonacci.R200` at x10 speed.

<img width="750" src="img/R200-EMU.gif" />

Preprocessor functions: removes comments and empty lines, resolves names of constants, variables and labels.

### Example
```
;;;; File: prog.R200
;;;; testing program
;;;; counts from 0 to COUNT in RA 

;;;;const's and var's declarations
const COUNT 5		;'const' is a key-word that binds name 'COUNT' to address 0x0 
			;of CONST memory and puts value 5 in there by that address
var TEMP		;'var' is a key-word that binds name 'TEMP' to address 0x0 of RAM

;;;;Start of the program
	mov RB, 0	;load value 0 to RB
	movm TEMP, RB 	;store RB to RAM word by address 'TEMP' (i.e. 0x0)
	movm RA, TEMP	;load RAM word by address 'TEMP' (i.e. 0x0) to RA
loop:			;'loop' is a label name. Preprocessor binds name 'loop' to  
			;address 0x1 of CONST memory and puts value 0x01
                  	;(ROM address the label is points to) in there by that address
	movc RB, COUNT 	;load value from CONST memory by address 'COUNT' (i.e. 0x0) to RB
	sub RB		;RB := RB-RA 
	snz		;skip next instruction if ZF==0 (result of previous ALU instruction is not zero)
	jmp exit	;load value from CONST memory by address 'exit' (i.e. 0x2) to PC
	inc RA 		;RA := RA+1
	jmp loop	;load value from CONST memory by address 'loop' (i.e. 0x1) to PC
exit:			;'exit' is a label name. Preprocessor binds name 'exit' to address 0x2 of CONST memory 
			;and puts value 0x07 (ROM address the label is points to) in there by that address
	halt		;stop the machine clock
```
Run step-by-step: `remu.exe prog.R200`, run until `halt`: `remu.exe prog.R200 run`.

At every cycle the emulator outputs its curent state. For instance:
```
Cycle: 38		--- machine cycle counter
CMD: halt		--- last executed instruction
State:			--- current state of registers, RAM and ALU flags
RA: 5[0b101, 0x5], RB: 0[0b0, 0x0], LEAF: 0, PC: 10
RAM: 0 0 0 0 0 0 0 0
c:0, z:1, skip:0, bc:0
CONST: 5 3 9 0 0 0 0 0 0 0 0 0 0 0 0 0
```

### Notes

- All `const`’s and `var`’s must precede actual instructions in the file.
- It's recommended to use 4 or more characters to name your `const`’s, `var`’s and labels, so they don’t accidently overlap with instruction names.
- All numerical values should be decimal. Hex `0x` and Bin `0b` formats are not supported.
- Emulation stops on the first encounter of `halt` instruction.
- There’s no comprehensive error output or any error-checking functionality (such as name overlapping, memory bound violation, etc.). If something goes wrong, the program just crashes with unhandled exception.
- It is recommended to output the final result to RA and RB.
- Memory limits
  - `conts`'s and labels: no more than 16 per program (in total)
  - `var`'s: no more than 8 per program
  - instructions: no more than 64 per program
  
## Software ideas
Available testing programs: 
- `mul.R200` - multiplication (12-bit X 12bit = 24bit)
- `div.R200` - division (24-bit / 24-bit = 24-bit[result] 24-bit[reminder])
- `fibonacci.R200` - calculates Fibonacci number (12-bit) from 2nd to 18th

There’s also sqrt `sqrt.R200` program that implements Babylonian method, but unfortunately as for now I haven’t been able to fit it in 64 instructions (it’s 77 now), so it doesn’t work with the emulator.

  
## Current construction progress

- Blocks assembled, mounted and tested: PC register, PC increment, ROM, clock generator.
- Blocks assembled: ALU, RA register, RB register, RAM
- Blocks that need to be designed: instruction decoder

### Photo
<img width="850" src="img/cur.jpg" />

### Video
<a href="http://www.youtube.com/watch?feature=player_embedded&v=zwi_KmVjN6o" target="_blank"><img src="http://img.youtube.com/vi/zwi_KmVjN6o/0.jpg" width="240" height="180" border="10" /></a>

### More awesome home-built computers
[Homebrew Computers Web-Ring](https://www.homebrewcpuring.org/)
