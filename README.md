
# Blueprism Hackathon
This repository is my solution to the blueprism hackathon challenge. Bellow I explain how I interpreted the challenge, the tools and methodologies I use and why I use it, how I tackled the problem and I leave a few references I used.

# The challenge

The challenge is to produce a solution able to solve word-ladder puzzles. To do this, it is provided a starting word (source word) and an ending word (destination word) and a dictionary of words. The goal is to start in the source word and by changing one character at the time traverse the dictionary until the destination word is found, in the shortest "word-distance" possible.
The secondary goals are to do this as performant as possible without ever sacrificing the readability, scalability and testability of the code. I also tried to apply SOLID and DRY principles wherever possible.

# Tools used

The IDE I used was visual studio 2019, I also used StackEdit to edit this MD file and for version control I used Git, Sourcetree and Github. Since its a smaller and individual project I chose not use gitflow or any logs.

About the code, I used C# 8 since its the one that comes with .NET3.1.

I used a few NuGet packages:

- GuardClauses - makes all the verifications a little bit smaller and easier to read
- XUnit - the unit testing framework I feel most confortable with
- AutoFixture - used it to populate variables for the unit tests, to make my unit tests easier to write and refactoring-safe
- NSubstitute - mocks my dependencies (I prefer it to moq since its more readable and user-friendly)
- FluentAssertions - Improves assertions readability

I used the following design patterns and principles:

- Strategy pattern - although only one algorithm may be used to provide the solution, I had to test a few approaches without having to change code previously done
- SOLID - I tried to apply the SOLID principles for a cleaner and efficient code base
- DRY - I tried to DRY the code but without ever sacrificing readability
- KISS - I tried to make the code as simpler as I could

The PC I used was my work laptop, i7 with 16GB RAM but with a lot of background processes, since mine is currently broken.

# How I tackled the problem (the long story)

My first few commits were about setting up the source and test projects, as well as adding input validations and optimizing the dictionary (and respective unit tests). I wanted to get rid of that before I start to work on the algorithm. The dictionary optimization was about removing words of a different length since those were not useful to get to the solution.

Although I've used algorithms before, I've never dealt with search-related problems. 
To better understand the problem and difficulties, and since I had the time, I decided to do my first approach without looking for search algorithms. My first thought was to use graphs to try to find the words.

First, I started by creating a "mask" system where each character different from the corresponding destination word character would be replaced by "*" and the distance would be calculated by counting the asterisks. The idea was to run over all the words and find the path with least distance. Before completely implement this, I realized this approach was becoming overcomplicated and I abandoned it without ever committing it.

### First working solution

My second approach was about advancing for each node instead of just visiting it. I created what I would later name a "ladder step" which would be a possible solution, containing each of the words, where the last one would be its current iteration. It started out as a single ladder step with the source word and it would loop through each word in the dictionary, searching for all the words with only a character distance from it, which I called neighbors. Each of these neighbors would generate a new ladder step (i.e. a possible path) and repeat the process. If one of the neighbors turned out to be the destination word, the search would stop and it would return the current ladder step's list of words. If the path could not be found it would just return null. This was first working solution which I committed.

### Improvements to the solution

My third approach was about improving the code of my previous approach by turning it a recursive method and improving the overall code. It lost a few milliseconds (~2-5ms) of performance because the search would only end when all the ladder steps returned, exhausting all the possibliities, but what was lost as performance was gained as readability. I considered also using a boolean property to just return all the ladder steps when the solution was found but I think it would make it harder to read.

After that I decided to investigate [search algorithms](https://www.javatpoint.com/ai-uninformed-search-algorithms) and noticed I had just implemented [depth first search algorithm](https://towardsdatascience.com/top-algorithms-and-data-structures-you-really-need-to-know-ab9a2a91c7b5). At this point I also refactored my code to both adapt nomenclature to match the algorithm's and to allow me to implement new solutions without changing code (strategy design pattern). I also found various implementations in multiple languages (java, python, c++ and a few in c#) which helped me to improve my code a little bit.

The research indicated the performance of my implementation, for the worst case scenario,  (assuming it was correctly implemented) was of O(b<sup>d</sup>) for time complexity and O(bd) for space complexity, where b is the branching factor and d is the depth, i.e. b relates to the amount of neighbors and d is the level, which for this challenge is 4 (we have 4 letters and we want to change all 4 of them). Also according to the research the performance was good but still had to repeat all the work for each phase.

### Second working solution
This next solution was done after I read about search algorithms: I decided to implement bidirectional search algorithm which is the fastest and requires less memory. The disadvantages is the necessity to know the end goal (which we know) and the complexity to implement it. This algorithm works similar to the previous one, creating possible paths and iterating each of them until a solution is found, but instead of just starting at one end and try to find the other end, this algorithm starts at both ends and tries to find the node at which 2 solutions intersect, and this will be the shortest distance. Each node (word) visited is stored in a list with the distance (level) to the starting node, while the possible paths are stored in a queue.

I also investigated some implementations to get a better understanding of the algorithm. Unfortunately most implementations only provide the distance but can't provide the path.

I started by implementing the algorithm based on those implementations (I listed them in the link section) and made a few modifications, to keep the code DRY and more readable. After getting the shortest distance from the common word to each end, I would have to re-calculate the way back to each end and get the words (which I could not find any implementations). Since I had the visited nodes and the distances, I just had to get the node from one list which had a distance of 2 to another node in the other list. This way I could get the middle words of the ladder and just had to keep incrementing the distance until I reached both ends. 

This implementation had a performance of O(b<sup>d</sup>) for both space and time complexity and using the C# TimeStopWatch I noticed it was quite faster. Again, sacrificing a bit of performance, I refactored the code to keep it readable and DRY, and extracted an abstract class with methods common to both approaches.

# How I tackled the problem (the TL;DR story)

I started by experimenting and unknowingly implementing the depth first algorithm without doing research. I later researched a bit and improved the code so the nomenclature would match the algorithm's and I also found out there was a better solution. Then I implemented the bidirectional search algorithm and then I also refactored it to keep the code DRY and readable.
I used TDD throughout all the implementations.

### Performance in my machine (in debug mode)
non-recursive DFS
- after the refactoring I removed the implementation but was about 2-5ms faster than the recursive DFS (this behavior is explained in the long story)

recursive DFS (mean)
- 362 ms

BDS (mean)
- 25 ms

# Code architecture, standards and why I implemented things in such way

Regarding my coding methodology, I applied TDD while writing most of the code and my unit tests follow the following convention: "MethodName_**When**StateUnderTest_**Should**ExceptedBehavior".

Each file is organized in its corresponding folder: 

- interfaces are in the interfaces folder
- abstractions are in the abstractions folder
- exceptions are in the exceptions folder
- stratagies are in the stratagies folder
- services are in the services folder
- extension methods (custom guard clauses) in the extensions folder in their respective folder
- the models for each strategy are in the models folder and in their respective strategy folder

Methods are organized according to the visibility and microsoft nomenclature patterns were followed. All public methods have guard clauses and private methods have debug asserts. Public methods, models, interfaces and abstractions also have a small documentation to help the next developer extending the code.

To add a new algorithm, there is an interface to be implemented and there is also an abstracted base, which helps with most non-algorithmic operations. The unit tests also have an abstract class. Each strategy should override the ApplyAlgorithm method and only focus on how to get to the solution as quickly as possible, but always respecting the rules.

The main idea of this implementation is to keep things open for extension while keeping them closed to modifications as much as possible and also to keep the responsibilities properly separated. Each strategy should only be concerned with the algorithm, and leave the rest to the base.

The word ladder strategies receives a FileReadWriteService instance to remove the responsibility of interacting with files, making them more testable and avoiding any coupling with a specific implementation (a new implementation of the FileReadWriteService can read/write to/from a database or to/from the screen, for example). Therefore the paths should be properties of the FileReadWriteService.

The source and destination words are provided in the Solve method, so a single strategy instance can solve several word ladder puzzles. I chose to make the algorithm time measurement as a required feature of the strategies, since its useful to compare algorithms, and simply hiding the results if the instance running is not being debugged.

If the dictionary is empty, the strategies will attempt to get it from the FileReadWriteService, but in case the dictionary needs to be updated between puzzles the ReadDictionary method can be used. This method will replace the dictionary words with newer ones provided by the service.

Another concept of this solution is to fail as fast as possible if any parameter or setting is wrong, and throw a meaningful exception to be displayed to the user/developer so they can fix the issue.

The FileReadWriteService was intentionally left out of the unit tests. The file read/write operations could be extracted to overridable methods to test the remaining logic, but since there was no remaining  logic to test (that was not from the guardclause library) it was not necessary to create unit test file.

# Some Links I found useful
https://www.javatpoint.com/ai-uninformed-search-algorithms - study of multiple algorithms

https://towardsdatascience.com/top-algorithms-and-data-structures-you-really-need-to-know-ab9a2a91c7b5 - study of multiple algorithms

https://efficientcodeblog.wordpress.com/2017/12/13/bidirectional-search-two-end-bfs/ - implementation of BDS in C++

https://www.geeksforgeeks.org/word-ladder-set-2-bi-directional-bfs/ - implementation of BDS in C#