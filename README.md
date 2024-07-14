# Introduction Of Travel Agent Application (TravelWith)

## Overview

Your Co-Travel Friend
Traveling to new countries often comes with questions and uncertainties. TravelWith is your go-to companion designed to enhance your travel experience by providing personalized assistance and guidance from locals, right at your fingertips.

## Features
### Key Features:
1. Interactive Conversations: Engage in real-time conversations with a local agent who understands the destination's culture, language, and local tips.

2. Customized Recommendations: Receive personalized recommendations on attractions, dining, accommodations, and activities based on your preferences and interests.

3. Instant Assistance: Get immediate help with navigation, language translation, emergency contacts, and local customs to ensure a smooth travel experience.

4. Cultural Insights: Gain insights into local traditions, events, and etiquette to make the most out of your travel experience.

5. Community Engagement: Connect with a community of fellow travelers and locals to share experiences, tips, and recommendations.
   
### Next Phase Features
- Conversation history and user authentication.
  
- Able to book/rent flight, hotels, restaurants, etc... (include booking/canceling/tracking).

- Able to access to more local knowledges about traveling, sports (fishing, hiking, cycling, etc), location/shop/food reviews.

- Support images generation to be more helpful.

- Having better UIs for user experiences and interactions.

- Support voice translation to avoid language barrier issue.

- Buidling self-traveling experiences module, where user can share their experiences after a trip and will be used as a knowledge base for the agent.
## Limitations

Currently the travel agent is still in an early stage, so data range will be limited. The added knowledges are mainly for Singapore right now.
Some sample inputs that we would advise:
- I need a plan to Singapore for 3 days, can you give me a plan?
- Where to eat in Singapore?
- How Singaporean enjoy their weekend?
- What are the transportation in Singapore?

## How It Works
Step 1: Input a destination (city/country) and your plan's duration.

Step 2: Chat with Your Local Agent: Start a conversation with your assigned local agent who will guide you throughout your journey.

Step 3: Explore with Confidence: Receive personalized recommendations and assistance tailored to your travel needs, ensuring a memorable and stress-free trip.

## Benefits
- Personalized Guidance: Access insider knowledge and tips from locals to discover hidden gems and authentic experiences.

- Real-Time Support: Resolve travel-related queries and challenges promptly with the help of a knowledgeable local agent.

- Enhanced Travel Experience: Maximize your time abroad with curated recommendations and cultural insights, making every moment count.
# Technical Portion
## Technologies
- .NET, AWS Lambda, AWS Bedrock, Supabase.

## High-level Architecture
![Alt text](AWS-Bedrock-TravelWith-Architecture.svg)

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

### Supabase Preparation
1. Go to https://database.new/
2. Create new Organization and obtain the connection string or you can create table on the webpage.
3. Run this code to create the `knowledges_base` table on your database.
```sh
CREATE TABLE IF NOT EXISTS public.knowledges_base
(
    id integer NOT NULL DEFAULT nextval('knowledges_base_id_seq'::regclass),
    content text COLLATE pg_catalog."default" NOT NULL,
    embedding vector(1536),
    CONSTRAINT knowledges_base_pkey PRIMARY KEY (id)
)
```
4. Next, you will need a create a sematic search function. Execute the code below on your database.
```sh
CREATE OR REPLACE FUNCTION public.match_knowledges(
	query_embedding vector,
	match_threshold double precision,
	match_count integer)
    RETURNS SETOF knowledges_base 
    LANGUAGE 'sql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
  select *
  from knowledges_base
  where knowledges_base.embedding <=> query_embedding < 1 - match_threshold
  order by knowledges_base.embedding <=> query_embedding asc
  limit least(match_count, 200);
$BODY$;
```
5. You can use the `knowledges_base` csv as your base data. (In Data folder within the repo) or we have a seperate API to add knowledges into the database.
6. Finally, replace the connection string to the project's code.

### AWS Bedrock Preparation
1. Create new AWS Bedrock Agent -> [here](https://docs.aws.amazon.com/bedrock/latest/userguide/agents-create.html#:~:text=Console-,To%20create%20an%20agent,Agents%20section%2C%20choose%20Create%20Agent)
2. Choose Claude 3 Sonnet.
3. More important, you need to add the instructions for the Agent. If you don't your agent will have no idea what action group or actions should be taken. Sample below:
```sh
You are a travel agent, helping clients to plan their trip from their inputs,
retrieve flights or attractions or foods or restaurants.
```
4. Create an action group and name it as `travel_lookup_action_group` and add an action to do the sematic search as `sematic_search` with a parameter as `input` (string type). Adding a description for the action group also good, you can try:
```sh
Actions only for getting flights, accomodations, foods, restaurants information and not for knowledge obtion or save to database.
```
5. In order to allow AWS Bedrock and AWS Lambda communicate each other, you will need to set `Lambda:InvokeFunction` permission for the Agent(go to the lambda -> Configurations -> Permissions -> Add a Resource-based policy statements)
```sh
StatmentId-> can be something unique.
Principal-> bedrock.amazonaws.com
Action-> lambda:InvokeFunction
Conditions-> use the arn from the agent (arn:aws:bedrock:us-west-2:{account_id}:agent/{agent_id})
```
### Credential & IAM
You need to create your own IAM user and generate an access key from it. Don't forget to set necessary permission such as AWSBedrockFullAccess. This step will allow you obtain your own creadential and allow the WebApp communicate with the AWS Bedrock agent.

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
### Deploy Sematic Search to AWS Lambda
1. Install AWS Toolkit for Visual studio -> https://docs.aws.amazon.com/toolkit-for-visual-studio/latest/user-guide/setup.html.
2. Open your Visual Studio and select the WebApp project.
3. You will see the `Publish to AWS Lambda` option and select it. (ensure your aws profile has been saved in your local machine)
4. Select Zip for the Package type and choose `Create new function` if this is the first deploy.
5. Use `TripPlanningAssistant::TripPlanningAssistant.Function::FunctionHandler` as a function endpoint for Handler.
6. Click Upload.
   
#### Invoking the Lambda Function
You can invoke your Lambda function using the AWS Management Console, AWS CLI, or programmatically.
Using AWS CLI
```sh
aws lambda invoke --function-name your-lambda-function-name output.json
```
Or you can test it on a test sreen after deploying the AWS lambda using AWS Toolkit on VS.
### Deploy WebApp to AWS Elastic Beanstalk
1. Install AWS Toolkit for Visual studio -> https://docs.aws.amazon.com/toolkit-for-visual-studio/latest/user-guide/setup.html. (skip it if installed)
2. Open your Visual Studio and select the WebApp project.
3. You will see the `Publish to AWS` option and select it. (ensure your aws profile has been saved in your local machine)
4. Select Elastic Beanstalk (linux) and click on Publish button.
   
### Monitoring and Logging
Monitor and view logs for your Lambda function using AWS CloudWatch.

## Contact
If you have any questions or issues, please open an issue in the repository or contact [Philip](philip.pang@embedcard.com) or [Johhny](johnny21042010@gmail.com).
