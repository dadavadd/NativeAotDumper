# NativeAotDumper 🚀

![C#](https://img.shields.io/badge/language-C%23-blue.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)
![Status](https://img.shields.io/badge/status-working-brightgreen)

A simple yet powerful tool for dumping metadata from running processes compiled with **.NET NativeAOT**. Peek under the hood of any NativeAOT application in real-time!

---

### 📖 Table of Contents
* [What is it? 🤔](#what-is-it-)
* [Key Features 🌟](#key-features-)
* [How does it look? 📸](#how-does-it-look-)
* [Under the Hood ⚙️](#under-the-hood-️)
* [How to Run 🛠️](#how-to-run-️)
* [Project Structure 📂](#project-structure-)
* [Want to Contribute? ❤️](#want-to-contribute-️)


## What is it? 🤔

When a .NET application is compiled with **NativeAOT**, it becomes a native executable without any IL code or JIT compilation. This is fantastic for performance but makes analyzing the application much harder.

**NativeAotDumper** solves this problem. It attaches to a *running* process and, as if by magic, pulls all available information about types, methods, and more directly from its memory.

It can dump:
- **Type Information** (classes, structs, interfaces) with their full names.
- **MethodTable addresses**.
- **Methods, fields, and properties** of each type, along with their metadata (tokens, offsets).
- **Frozen Strings**, which the compiler places in a special memory region.

## Key Features 🌟

*   **🔥 Real-time Analysis:** Works with running processes, allowing you to inspect the application's state on the fly.
*   **🧠 Deep Metadata Diving:** Includes a complete implementation of a .NET Native Format parser, adapted to read from process memory. This isn't just reading a few structs; it's a full-fledged parsing of complex, nested metadata.
*   **💅 Simple UI:** A user-friendly WinForms interface to display the dumped information clearly. No console madness, everything is right in front of you!
*   **🧩 Clean Architecture:** The project is divided into a core library (`NativeAotDumper.Core`) and a user interface (`NativeAotDumper`), making it easy to extend.

## How does it look? 📸

Imagine a window split into two parts:

| Type & Member Dump                                                                    | Frozen String Dump                                     |
| ------------------------------------------------------------------------------------- | ------------------------------------------------------ |
| On the left, you see a tree of all types found in the application. Expanding any type reveals a list of its fields, properties, and methods, including their tokens and memory offsets. It's a goldmine for reverse engineering! | On the right, a long list of all string literals found in the special "frozen" region. Useful for finding keys, paths, or other interesting text data. |

![image](https://github.com/user-attachments/assets/736cfbb2-d03d-4cae-ba62-140c4aae8166)

## Under the Hood ⚙️

The entire process can be broken down into these steps:

1.  **Find the Process:** `ProcessMemoryReader` finds the target process by name and gets a handle to read its memory.
2.  **Find the `ReadyToRun` Header:** `ReadyToRunHeaderParser` scans the memory for the `'RTR\0'` signature to find the entry point for NativeAOT metadata.
3.  **Parse Module Sections:** `ModuleInfoParser` reads information about all sections (like `FrozenObjectRegion`, `TypeMap`, `MetadataBlob`, etc.).
4.  **Extract Strings:** `FrozenStringParser` iterates through the `FrozenObjectRegion` section and extracts all strings by comparing their vtable with a reference one.
5.  **Metadata Magic:** `MetadataResolver` does the heavy lifting:
    *   Reads the `TypeMap` hash table to map MethodTable addresses to type handles.
    *   Using a custom `MetadataReader`, it parses the binary blob containing the metadata (`MetadataBlob`).
    *   Recursively builds the full names of types by traversing namespaces.
    *   Gathers all the information into convenient `RuntimeTypeInfo` structs.
6.  **Display the Results:** `MainForm` simply displays all the collected information in `ListBox` controls.

## How to Run 🛠️

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/dadavadd//NativeAotDumper.git
    ```
2.  Open the `NativeAotDumper.sln` solution in **Visual Studio**.
3.  **‼️ Important Step:** Open `NativeAotDumper/MainForm.cs` and change the target process name. It's currently set to a placeholder.
    ```csharp
    // NativeAotDumper/MainForm.cs

    public partial class MainForm : Form
    {
        private readonly Dumper _parser;

        public MainForm()
        {
            InitializeComponent();
            
            // 👇 You need to change this line!
            _parser = new("YourProcessNameHere"); // e.g., "NativeAOTProcess.exe"
            
            // ... rest of the code
        }
    }
    ```
4.  Build and run the project (`F5`).
5.  Make sure the target application (the one you specified in step 3) is already running.

## Project Structure 📂

<details>
<summary>Click to expand</summary>

```
└── ./
    └── NativeAotDumper
        ├── NativeAotDumper (UI Project, WinForms)
        │   ├── MainForm.cs
        │   ├── MainForm.Designer.cs
        │   └── Program.cs
        └── NativeAotDumper.Core (Core Logic)
            ├── Enums
            ├── NativeAot
            │   ├── Extensions
            │   ├── Interfaces
            │   ├── Parsers (Header and Section Parsers)
            │   ├── Utils (Main metadata parser and its dependencies)
            │   │   ├── Metadata
            │   │   ├── NativeFormat
            │   │   └── MetadataResolver.cs
            │   ├── Dumper.cs (Main Class)
            │   └── ProcessMemoryReader.cs
            └── Structs (Structs for in-memory data)
                ├── NativeAot
                └── ReadyToRun
```
</details>

## Want to Contribute? ❤️

All contributions are welcome! Feel free to fork, create branches, and send Pull Requests. If you find a bug or have an idea for a new feature, please create an Issue.

1.  Fork the Project.
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`).
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`).
4.  Push to the Branch (`git push origin feature/AmazingFeature`).
5.  Open a Pull Request.

## License 📄

This project is distributed under the MIT License. See `LICENSE` for more information.

## Acknowledgements 🙏

*   A huge thanks to the .NET team for making the `dotnet/runtime` source code open. Much of the metadata parsing logic was lovingly adapted from there.
*   To everyone who inspires the creation of tools like this.
