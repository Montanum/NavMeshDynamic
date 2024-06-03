# Navigation Mesh Dynamic Tool (NMD Tool)

## Overview
The Navigation Mesh Dynamic Tool (NMD Tool) is an advanced solution designed to address the challenges of real-time navigation mesh generation for AI agents in 3D game environments. Traditional navigation meshes are static and quickly become outdated in dynamic game worlds. The NMD Tool overcomes this limitation by generating and updating navigation meshes in real-time, ensuring accurate and efficient pathfinding for AI agents.


![PROJECT_POSTER](https://github.com/Montanum/NavMeshDynamic/assets/62666688/58983eff-e38d-47f4-96e6-fa664a77f33e)


## PROJECT VIDEO
--VIDEO PLACEHOLDER

## Project Details
**Title:** Navigation Mesh Dynamic Tool  
**Supervisor:** Ali Seydi Keçeli  
**Group Members:** Kerem Aydoğan, Kübra Özdamar  

## Abstract
The NMD Tool is a sophisticated tool developed to generate and update navigation meshes dynamically in real-time for procedurally expanding game environments. Unlike traditional static navigation meshes, the NMD Tool ensures that AI agents can navigate efficiently even as the game world evolves. The tool employs advanced algorithms, parallel computing techniques, and hierarchical pathfinding strategies to maintain optimal performance and responsiveness.

## Features
- **Real-Time Navigation Mesh Generation:** Dynamic creation and updating of navigation meshes as the game environment changes.
- **Hierarchical Pathfinding:** Utilizes a hierarchical leveled area system to efficiently manage and process navigation data.
- **Parallel Processing:** Leveraging Unity’s C# Job System and Burst Compiler for enhanced computational performance.
- **A* Pathfinding Algorithm:** Ensures intelligent navigation decisions by AI agents based on the updated navigation mesh.

## Methodology
1. **Data Ingestion and Preprocessing:** Collection and optimization of 3D game environment data, including terrain, obstacles, and waypoints.
2. **Dynamic Map Adaptation:** Algorithms to generate navigation meshes in real-time as new areas are added to the game environment.
3. **Navigation Mesh Generation:** Utilizes a hierarchical leveled area system to create and organize nodes into a coherent navigation mesh.
4. **Parallel Programming Integration:** Employs Unity’s C# Job System and Burst Compiler to distribute computational tasks efficiently.
5. **Pathfinding Algorithm Implementation:** Integration of the A* pathfinding algorithm to enable intelligent navigation decisions by AI agents.

## Results
The NMD Tool was rigorously tested to ensure its efficiency and impact on game performance. The tool demonstrated significant improvements in AI navigation efficiency and responsiveness. Performance evaluations confirmed the tool's ability to operate efficiently without compromising game performance, making it suitable for a wide range of gaming scenarios.

## Impact and Future Directions
The NMD Tool is expected to revolutionize AI navigation in dynamic game environments, providing more immersive gameplay experiences. Future directions include:
- Further optimization of the tool for enhanced performance.
- Integration with broader game development pipelines.
- Exploration of advanced pathfinding strategies, such as dynamic obstacle avoidance and real-time updates to existing areas of the navigation mesh.


## License
Shield: [![CC BY-ND 4.0][cc-by-nd-shield]][cc-by-nd]

This work is licensed under a
[Creative Commons Attribution-NoDerivs 4.0 International License][cc-by-nd].

[![CC BY-ND 4.0][cc-by-nd-image]][cc-by-nd]

[cc-by-nd]: https://creativecommons.org/licenses/by-nd/4.0/

[cc-by-nd-image]: https://licensebuttons.net/l/by-nd/4.0/88x31.png
[cc-by-nd-shield]: https://img.shields.io/badge/License-CC%20BY--ND%204.0-lightgrey.svg
