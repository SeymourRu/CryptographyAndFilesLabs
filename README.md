# CryptographyAndFilesLabs

Special project which contains all implemented labs in 2nd term of MA(T)I magister programm

Project have 2 main lab streams: 
  "Mathematical foundations of information security and information security" (kind of cryptography~) 
                                 and 
  "Modern methods of information storage and processing"

#Way it works
Each task in each homeworke implemented as a separate file.Avaliable tasks will be resolved by Autofac itself at runtime. This can be configured in Programm.cs

#Implemented tasks
All tasks are located in CoreDefinitions\Tasks(let`s call it basepath). Here is a list of implemented tasks:

Mathematical foundations of information security and information security:
	Path													Definition
basepath\Cryptography\Crypto_Task1_1.cs			A set is given, a permutation is given, and the set is derived using a permutation.
basepath\Cryptography\Crypto_Task1_2.cs			Given a set, set a mixed set, output a permutation.
basepath\Cryptography\Crypto_Task1_2.cs			Theorem: any set can be represented by decomposition into products of transpositions. Let's try to find a cyclic permutation and build a transposition.

basepath\Cryptography\Crypto_Task2_1.cs			ρ - Pollard method (ordinary and parallel). Aim: find an x such that g ^ x = a % m
basepath\Cryptography\Crypto_Task2_1.cs			ρ - Pollard method (ordinary and parallel). Aim: find an x such that g ^ x = a % m

basepath\Cryptography\Crypto_Task3_1.cs			Fermat primality test
basepath\Cryptography\Crypto_Task3_2.cs			Rabin-Miller primality test
basepath\Cryptography\Crypto_Task3_3.cs			Pollard's p − 1 factorization algorithm
basepath\Cryptography\Crypto_Task3_4.cs			Pohling-Hellman`s algorithm

basepath\Cryptography\Crypto_CourseWork.cs			Dixon's factorization algorithm

Modern methods of information storage and processing:
	Path													Definition
basepath\MethodsOfFilesProcessing\Files_Task1_1.cs		There is some database. If you know the probability distribution of query records, you can order the data in descending order of these probabilities, and thus, to achieve a more optimal search. The program reorders an array of integer keys according to a given probability distribution. Such distributions as geometric, binomial and wedge are used.
basepath\MethodsOfFilesProcessing\Files_Task1_2.cs		Self-organized file generation + some test readings
basepath\MethodsOfFilesProcessing\Files_Task1_3.cs		Search in Self-organized file

basepath\MethodsOfFilesProcessing\Files_Task2_1.cs		DONALD E. KNUTH. The Art of Computer Programming. Volume 3 Sorting and searching. Paragraph 6.2.2, algorithm T (search for a tree with an insert)
basepath\MethodsOfFilesProcessing\Files_Task2_2.cs		DONALD E. KNUTH. The Art of Computer Programming. Volume 3 Sorting and searching. Paragraph 6.2.2, algorithm D (removal of a tree node)
basepath\MethodsOfFilesProcessing\Files_Task2_3.cs		DONALD E. KNUTH. The Art of Computer Programming. Volume 3 Sorting and searching. Paragraph 6.2.3, algorithm A (search with insertion on a balanced tree)