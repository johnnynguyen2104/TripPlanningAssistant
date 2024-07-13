# .NET Lambda Application

## Overview

This is a .NET Lambda application designed to connect a Bedrock Agent with a Supabase database to retrieve information about flights, accommodations, restaurants, and attractions. The primary feature of the application is trip planning.

## Features

- Trip Planning: Retrieve and manage data related to flights, accommodations, restaurants, and attractions.

## Prerequisites

Before you begin, ensure you have met the following requirements:

- .NET SDK installed (version 8.0 or later)
- AWS CLI installed and configured
- An AWS account with permissions to create and manage Lambda functions
- Access to a Supabase database

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
