# LibrarySearch.AI: Intelligent Book Discovery

**LibrarySearch.AI** is a high-performance web application designed to solve the challenge of unstructured book searches, known as **"Blobs"**. By combining **Clean Architecture**, **SOLID principles**, and **Generative AI (LLM)**, the system transforms messy user inputs into precise library matches.

The project uses **Google Gemini 3 Flash Preview** for semantic reasoning and the **Open Library API** for real-world book data.

---

## Getting Started

### Prerequisites
* **.NET 8.0 SDK** or higher.
* **Google Gemini API Key**: You can obtain one at [Google AI Studio](https://aistudio.google.com/).

### Configuration (User Secrets)
To protect sensitive data and prevent API keys from being leaked to version control, this project utilizes **Secret Manager**.

To run the application, you **must** configure your API Key locally:

1. Right-click on the `LibrarySearch.Web` project in Visual Studio.
2. Select **"Manage User Secrets"**.
3. Paste the following JSON, replacing the value with your personal key:

```json
{
  "Gemini": {
    "ApiKey": "YOUR_ACTUAL_API_KEY_HERE"
  }
}

---

## Architecture Deep Dive

This project isn't just a standard MVC; it implements **Clean Architecture** to separate concerns and ensure testability.

### The Search Workflow:
1. **Request**: The user submits a "Messy Blob" (e.g., "Tolkien Hobbit 1937").
2. **AI Layer (LLM)**: `GeminiAiExtractionService` parses the blob into structured data (Title: "The Hobbit", Author: "Tolkien").
3. **Domain Logic**: The `MatchBooksUseCase` orchestrates the search.
4. **Resilience Hierarchy**: If multiple results are found, the system applies **Liskov-compliant Strategies** to select the best match.
5. **Fallback**: If no exact match is found, the system provides "Suggested Matches" instead of an empty screen.



---

## Unit Testing Strategy

With a **90.6% line coverage**, the project ensures stability across all critical paths:

* **Domain Tests**: Validation of the `Strategy Pattern` and `NormalizedText` (text cleaning logic).
* **Application Tests**: Mocking the AI Service with **Moq** to test search orchestration without costs or internet dependency.
* **Infrastructure Tests**: Verifying that API responses from Open Library are correctly mapped to our Domain Entities.

| Metric | Result |
| :--- | :--- |
| **Line Coverage** | 90.6% |
| **Branch Coverage** | 65.9% |
| **Frameworks** | xUnit, Moq, FluentAssertions |



---

## Running the Tests

To verify the quality of the project yourself, run the following command in the terminal:

```bash
dotnet test --collect:"XPlat Code Coverage"