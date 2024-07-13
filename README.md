# Introduction Of Travel Agent Application (TravelWLocal)

## Overview

Your Co-Travel Friend
Traveling to new countries often comes with questions and uncertainties. Travel with Local is your go-to companion designed to enhance your travel experience by providing personalized assistance and guidance from locals, right at your fingertips.

## Features
### Key Features:
1. Interactive Conversations: Engage in real-time conversations with a local agent who understands the destination's culture, language, and local tips.

2. Customized Recommendations: Receive personalized recommendations on attractions, dining, accommodations, and activities based on your preferences and interests.

3. Instant Assistance: Get immediate help with navigation, language translation, emergency contacts, and local customs to ensure a smooth travel experience.

4. Cultural Insights: Gain insights into local traditions, events, and etiquette to make the most out of your travel experience.

5. Community Engagement: Connect with a community of fellow travelers and locals to share experiences, tips, and recommendations.
   
### Next Phase Features
- Able to book/rent flight, hotels, restaurants, etc... (include booking/canceling/tracking).

- Able to access to more local knowledges about traveling, sports (fishing, hiking, cycling, etc), location/shop/food reviews.

- Support images generation to be more helpful.

- Having a UI for better user experiences and interactions.

- Support voice translation to avoid language barrier issue.

- Buidling self-traveling experiences module, where user can share their experiences after a trip and will be used as a knowledge base for the agent.

## How It Works
Step 1: Choose Your Destination: Select your destination city or country.

Step 2: Chat with Your Local Agent: Start a conversation with your assigned local agent who will guide you throughout your journey.

Step 3: Explore with Confidence: Receive personalized recommendations and assistance tailored to your travel needs, ensuring a memorable and stress-free trip.

## Benefits
- Personalized Guidance: Access insider knowledge and tips from locals to discover hidden gems and authentic experiences.

- Real-Time Support: Resolve travel-related queries and challenges promptly with the help of a knowledgeable local agent.

- Enhanced Travel Experience: Maximize your time abroad with curated recommendations and cultural insights, making every moment count.
# Techical Portion
## Prerequisites

Before you begin, ensure you have met the following requirements:

- .NET SDK installed (version 8.0 or later)
- AWS CLI installed and configured
- An AWS account with permissions to create and manage Lambda functions
- Access to a Supabase database

## Improvements
1. Need to store secrets information to somewhere safer.
2. Refine the code and create mapping class for data response.
3. Support better response for user easier to read.

## Getting Started

### Clone the Repository

```sh
git clone https://github.com/johnnynguyen2104/TripPlanningAssistant.git
cd your-repo-name
```

### Install Dependencies
```sh
dotnet restore
```

## Configuration
Configure your AWS credentials using the AWS CLI:
```sh
aws configure
```
Provide your accessKey and secretKey accordingly from you AWS account.
Also copy and paste your AWS credential and Supabase key into the `Function.cs`.
## Deployment
### Build the Application
```sh
dotnet build
```
### Deploy to AWS Lambda
Use the AWS CLI or AWS Management Console to create and deploy your Lambda function.
Using AWS CLI
1. Package the application:
```sh
dotnet lambda package -o your-lambda-package.zip
```
2. Deploy the packaged application:
```sh
aws lambda update-function-code --function-name your-lambda-function-name --zip-file fileb://your-lambda-package.zip
```
### Usage
#### Invoking the Lambda Function
You can invoke your Lambda function using the AWS Management Console, AWS CLI, or programmatically.
Using AWS CLI
```sh
aws lambda invoke --function-name your-lambda-function-name output.json
```
### Monitoring and Logging
Monitor and view logs for your Lambda function using AWS CloudWatch.

## Contact
If you have any questions or issues, please open an issue in the repository or contact [Philip](philip.pang@embedcard.com) or [Johhny](johnny21042010@gmail.com).
