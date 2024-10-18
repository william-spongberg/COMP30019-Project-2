# Project 2 Report

## Table of Contents

- [Evaluation Plan](#evaluation-plan)
- [Evaluation Report](#evaluation-report)
- [Shaders and Special Effects](#shaders-and-special-effects)
- [Summary of Contributions](#summary-of-contributions)
- [References and External Resources](#references-and-external-resources)

## Evaluation Plan

### 1. Evaluation Techniques:
We will use a combination of querying and observational techniques to evaluate the usability of our game. The primary techniques will include:

#### Post-Study System Usability Questionnaire (PSSUQ):
- Overall, I am satisfied with how easy it is to use this system. (Usefulness)
- It was simple to use this system. (Usefulness)
- I was able to complete the tasks and scenarios quickly using this system. (Usefulness)
- I felt comfortable using this system. (Usefulness)
- It was easy to learn to use this system. (Usefulness)
- I believe I could become productive quickly using this system. (Usefulness)
- The system gave error messages that clearly told me how to fix problems. (Information Quality)
- Whenever I made a mistake using the system, I could recover easily and quickly. (Information Quality)
- The information (such as online help, on-screen messages, and other documentation) provided with this system was clear. (Information Quality)
- It was easy to find the information I needed. (Information Quality)
- The information was effective in helping me complete the tasks & scenarios. (Information Quality)
- The organization of information on the system screens was clear. (Information Quality)
- The interface of this system was pleasant. (Interface Quality)
- I liked using the interface of this system. (Interface Quality)
- This system has all the functions and capabilities I expect it to have. (Interface Quality)
- Overall, I am satisfied with this system. (Interface Quality)

#### System Usability Scale (SUS):
To complement the PSSUQ, we will also use SUS with a different group of participants. It consists of 10 questions, measured on a 5-point Likert Scale ranging from Strongly Disagree to Strongly Agree. 
1. I think that I would like to use this system frequently.
2. I found the system unnecessarily complex.
3. I thought the system was easy to use.
4. I think that I would need the support of a technical person to be able to use this system.
5. I found the various functions in this system were well integrated.
6. I thought there was too much inconsistency in this system.
7. I would imagine that most people would learn to use this system very quickly.
8. I found the system very cumbersome to use.
9. I felt very confident using the system.
10. I needed to learn a lot of things before I could get going with this system.

**Scoring:**
- Odd-numbered questions (1, 3, 5, 7, 9) reflect positive aspects of usability. To score these,we need to subtract 1 from the user's response.
- Even-numbered questions (2, 4, 6, 8, 10) reflect negative aspects of usability. To score these, we need to subtract the user's response from 5.
- Sum the scores for all 10 questions to get a number between 0 and 40. We need to multiply this sum by 2.5 to convert it into a score out of 100.

#### Observational Evaluation:
We will observe participants as they interact with the game, noting any difficulties they encounter, such as confusion with controls, tasks, or UI elements, and also assess areas where they get stuck or might need our help to understand how to proceed. We will use this to assess real-time user experience and identify areas of improvement in gameplay.

### 2. Tasks for Participants:
We will ask participants to perform specific tasks when they’re playing the game so they will know what to focus on and it will make it more effective in answering questions.
- **Complete the First Level:** Ask them to take a break after completing level 1, and ensure they’re understanding the story of the game as they go forward. See if they have any questions, and try to find solutions so that other users might not face the same difficulties.
- **Defeat an Enemy:** This will evaluate how well the player can use game mechanics for combat.
- **Fight the Boss:** Different players may lose/win which will help us get different points of view on how they felt during/after the final fight.

### 3. Participants:
We will recruit 10 or more participants:
- **Group 1:** Will complete the PSSUQ and be observed while performing the tasks.
- **Group 2:** Will complete the SUS and be observed while performing the tasks.

#### Recruitment Strategy:
Participants will be recruited from the target audience for the game (e.g., those who love to be immersed in a fantastical yet eerie and confusing environment). Participants will be recruited based on a basic knowledge of:
- Basic gaming experience (at least familiar with WASD controls and mouse for navigation).
- Aged 16 years or older to align with the target demographic.
We will recruit participants using social media platforms, gaming communities, and university forums.

### 4. Data Collection:
- **PSSUQ and SUS:** After completing the tasks, participants will be asked to fill out the PSSUQ and SUS questionnaires, rating each item on a scale. We will be using Microsoft Forms to collect data from the participants to ensure that the data formal is the same for each participant and can be analysed properly after collection. 
- **Observational Data:** We will use video recording and note-taking (based on consent from the player) to document interactions with the game, focusing on:
  - Time taken to complete tasks.
  - Areas where participants get stuck or express frustration.
  - Verbal feedback during gameplay to capture spontaneous reactions.
  - Post-game general question about what they thought about the game, and get an idea of what they think the areas of improvement are.

### 5. Data Analysis:
- **PSSUQ Analysis:** We will calculate the average scores for each of the three subscales (System Usefulness, Information Quality, and Interface Quality) and overall usability. Each participant’s scores will be aggregated, and we will look for trends in the data.
- **SUS Analysis:** The SUS responses will be converted into a usability score for each participant. The SUS score will range from 0 to 100, with a higher score indicating better usability.
- **Observational Data:** We will analyze the recorded gameplay to identify recurring issues, challenges with controls, and frustration points. Specific metrics include:
  - **Task Completion Rate:** Percentage of participants who successfully completed each task.
  - **Task Completion Time:** Average time taken to complete tasks.
  - **Error Rate:** Number of times participants made errors while interacting with the game (e.g., wrong keys, confusion with mechanics).
  - **Observational Notes:** Identify any non-verbal cues indicating confusion, frustration, or ease.

### 6. Timeline:
- **Week 1 (7th to 14th October):** Recruit participants and finalize the evaluation tasks.
- **Week 2 (15th to 21st October):** Conduct the evaluation with the participants.
- **Week 3 (21st to 29th October):** Analyze the data and make the necessary changes.

### 7. Responsibilities:
We will divide the responsibilities for working on the evaluation plan accordingly: Three people will be responsible for recruiting participants and collecting data while one person will be analysing the data found. After analysis, the team will work together to ensure that the game is modified according to the feedback received.

## Evaluation Report

TODO (due milestone 3) - see specification for details

## Shaders and Special Effects

TODO (due milestone 3) - see specification for details

## Summary of Contributions

William Spongberg:
  - Managed the project and coordinated the team AlphaBeta.
  - Created the initial project structure and set up the Unity project.
  - Using Blender, created from scratch 3D models for the desk and CRT monitor, and then implemented them into prefabs with appropriate materials.
  - Developed, designed and implemented procedural world generation for an infinite office space using Perlin noise. Created all related scripts and prefabs.
  - Implemented post processing effects using the built-in Global Volume to drastically enhance the visual quality of the game and its "eerie" atmosphere.
  - Implemented various bug fixes for known issues in the project. See Git commits for more details.
  - Implemented particle system for the muzzle flash and blood effects.
  - Implemented the FreeCam prefab, a smooth free-look camera system akin to the camera used in Scene view that was used for the game's trailer.
  - Implemented ObjectSpawner, a script that spawns objects randomly within a given range of the player.
  - Implemented the initial very basic enemy AI using NavMesh and NavMeshAgent.
  - Implemented automatic NavMesh baking around the player to ensure that enemies can navigate the environment.
  - Repeatedly tweaked procedural world gen and AI baking to ensure the game performs well.
  - Implemented GlitchShader.shader, a shader that creates a complex 'glitchy' effect for the NPCs using gpu instancing, dithering and random offsets of the UVs and x coordinates.
  - Numerous other small scripts and tweaks to improve the game's performance, visual quality, and overall experience.
---
Alistair Wern Hao Cheah
 - TODO
---
David Kee Siong Chin
 - TODO 
---
Ananya Agarwal
 - TODO

## References and External Resources

### Assets
- [Synty student bundle](https://assetstore.unity.com/student-plan-pack1): For various low-poly 3D models, textures, and materials.
- [Unity Starter Assets](https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-updates-in-new-charactercontroller-pa-196526): For NPC movement animations and environment materials.

### Resources
- TODO (e.g tutorials, etc)
