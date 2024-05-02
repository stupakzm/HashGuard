# HashGuard

HashGuard is a console application in C# designed to calculate and compare hash values of text and files using different cryptographic hash algorithms. It offers various modes to handle text and file inputs and compare their hash values for integrity checks.

## Features

- **Text Hashing**: Calculate hash values for direct text input using multiple hash algorithms.
- **File Hashing**: Generate hash values for file content, supporting shortcut resolution using UtilityKit.
- **Compare Texts**: Compare hash values of two input texts to check their integrity and similarity.
- **Compare Files**: Compare hash values of two files for integrity verification.

## Supported Hash Algorithms

- MD5
- SHA1
- SHA256
- SHA384
- SHA512

## Prerequisites

Ensure that you have the .NET environment set up on your machine to run C# applications. Also, the program uses `UtilityKit.dll` for resolving shortcuts which should be properly referenced in your project. You can find more about `UtilityKit` and its functionalities [here](#UtilityKit-Link).
