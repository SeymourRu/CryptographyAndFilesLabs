# CryptographyAndFilesLabs

Special project which contains all implemented labs in 2nd term of MA(T)I magister programm

Project have 2 main lab streams:

  >"Mathematical foundations of information security and information security" (kind of cryptography~) 

											and

  >"Modern methods of information storage and processing"

## Way it works ##

Each task in each homeworke implemented as a separate file.Avaliable tasks will be resolved by Autofac itself at runtime. This can be configured in Programm.cs

## Implemented tasks ##

All tasks are located in CoreDefinitions\Tasks(let`s call it basepath). Here is a list of implemented tasks:

### Mathematical foundations of information security and information security ###

Path | Definition
--------------------------------------------------------|----------------------------------------------------
___basepath\Cryptography\Crypto_Task1_1.cs___|A set is given, a permutation is given, and the set is derived using a permutation.
___basepath\Cryptography\Crypto_Task1_2.cs___|Given a set, set a mixed set, output a permutation.
___basepath\Cryptography\Crypto_Task1_2.cs___|Theorem: any set can be represented by decomposition into products of transpositions. Let's try to find a cyclic permutation and build a transposition.
___basepath\Cryptography\Crypto_Task2_1.cs___|ρ - Pollard method (ordinary and parallel). Aim: find an x such that g ^ x = a % m
___basepath\Cryptography\Crypto_Task2_1.cs___|ρ - Pollard method (ordinary and parallel). Aim: find an x such that g ^ x = a % m
___basepath\Cryptography\Crypto_Task3_1.cs___|Fermat primality test
___basepath\Cryptography\Crypto_Task3_2.cs___|Rabin-Miller primality test
___basepath\Cryptography\Crypto_Task3_3.cs___|Pollard's p − 1 factorization algorithm
___basepath\Cryptography\Crypto_Task3_4.cs___|Pohling-Hellman`s algorithm
___basepath\Cryptography\Crypto_CourseWork.cs___|Dixon's factorization algorithm

### Modern methods of information storage and processing: ###

Path|Definition
--------------------------------------------------------|----------------------------------------------------
___basepath\MethodsOfFilesProcessing\Files_Task1_1.cs___|There is some database. If you know the probability distribution of query records, you can order the data in descending order of these probabilities, and thus, to achieve a more optimal search. The program reorders an array of integer keys according to a given probability distribution. Such distributions as geometric, binomial and wedge are used.
___basepath\MethodsOfFilesProcessing\Files_Task1_2.cs___|Self-organized file generation + some test readings
___basepath\MethodsOfFilesProcessing\Files_Task1_3.cs___|Search in Self-organized file
___basepath\MethodsOfFilesProcessing\Files_Task2_1.cs___|DONALD E. KNUTH. The Art of Computer Programming. Volume 3 Sorting and searching. Paragraph 6.2.2, algorithm T (search for a tree with an insert)
___basepath\MethodsOfFilesProcessing\Files_Task2_2.cs___|DONALD E. KNUTH. The Art of Computer Programming. Volume 3 Sorting and searching. Paragraph 6.2.2, algorithm D (removal of a tree node)
___basepath\MethodsOfFilesProcessing\Files_Task2_3.cs___|DONALD E. KNUTH. The Art of Computer Programming. Volume 3 Sorting and searching. Paragraph 6.2.3, algorithm A (search with insertion on a balanced tree)