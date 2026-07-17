# SignVR
A Real-Time English to American Sign Language (ASL) translation software for Meta Quest 3.

<video src="https://github.com/user-attachments/assets/baa2cb5f-c642-4c72-b4c1-2a983e0ad2fa" width="50%" autoplay></video>

<video src="https://github.com/user-attachments/assets/95892ec5-e4cb-45df-9d3e-f8a069ce50d5" width="50%" autoplay></video>

<video src="https://github.com/user-attachments/assets/8f4705d6-1d8d-4a4e-93d1-e515e91bac31" width="50%" autoplay></video>

## ⚠️ READ THIS BEFORE YOU CLONE
Due to the large size of files stored in this repo, you'll need Git LFS to download it correctly.

* Get Git LFS [Here](https://git-lfs.com/)
* `git lfs clone https://github.com/Davide-Leotta/SignVR.git`

## What is SignVR?
SignVR is a translation tool from English to ASL and Gloss notation, developed in Unity 6000.2.8f1 for the Meta Quest VR.

## Why was SignVR developed?
This project was developed as an assignment for the "Mixed Reality and Wearable Vision" class in University of Catania's Computer Science Course. The software was not built to be a released commercial product.

## Who developed SignVR?
SignVR was made by a joint effort from @Davide-Leotta, @Paranojike and @fnxwarehouse.

## Tech used in SignVR
SignVR was made in Unity and C#. 

* **Meta XR SDK**
* **LLaMA 3.3 70B Versatile** (via Groq) was used for Gloss Notation translation.
* **Wit.AI** was used for Speech-To-Text integration.

## 🔑 Setup & API Keys
For security reasons before uploading the repository to GitHub, the API keys required for the LLM connection on Groq and the Wit.AI service have been removed. 
To run the software correctly, you will need to generate your own API keys and insert them into the project files:

1. **Groq (LLaMA 3.3)**: 
   * Navigate to `SignVR/SignVR/Assets/MetaXR/LargeLanguageModels_HuggingFace_ProviderProfile.asset`.
   * Open the file and paste your API key in the empty `apiKey:` field.
2. **Wit.AI (Speech-To-Text)**: 
   * Navigate to `SignVR/SignVR/ProjectSettings/wit.config`.
   * Open the file, replace `"INSERISCI_IL_TUO_APP_ID"` with your Wit.AI App ID, and fill the empty `"serverToken":""` field with your server token.

### ⚠️ Important Notice: No Pre-built APK
For security reasons, we have decided **not** to include a pre-built APK in this release. 

Distributing a functional application would require including personal API keys for **Groq (LLaMA 3.3)** and **Wit.AI (Speech-To-Text)** directly in the build, which poses a severe security risk.

To fully test and experience **SignVR**, you will need to build the project yourself:
1. Clone the repository using Git LFS.
2. Generate your own free API keys from Groq and Wit.AI.
3. Insert them into the Unity project as detailed in the `Setup & API Keys` section of our [README](#-setup--api-keys).
4. Build the APK directly via Unity and deploy it to your Meta Quest 3.

## Datasets for ASL Animations
SignVR uses animations from two datasets according to their licenses. You can find them here: 

* [SLMocapArchive](https://github.com/StudioGalt/Sign-Language-Mocap-Archive) @StudioGalt
* [3D-LEX v1.0: 3D Lexicons for American Sign Language and Sign Language of the Netherlands](https://arxiv.org/abs/2409.01901)

## What words are translated in SignVR?
SignVR has a vocabulary of 40 words and all the English alphabet. It can virtually translate any word via spelling.

## Where can I find SignVR docs?
Due to the scope of the project being an assignment for a university class, no developer-oriented documentation was made. You can find a report that explains in detail how the project was made, explained snippets of code and other information inside the `Docs` folder. A translation in English of the report is currently being made.

## 📄 License
This project is licensed under the **PolyForm Noncommercial License 1.0.0**. Please see the `LICENSE` file included in this repository for the full terms and conditions.
