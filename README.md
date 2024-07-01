# Origin Backend Take-Home Assignment
![System Design](system.png)

You are designing the user access management service of the microservices architecture above.

There are currently two ways that users can have access to the platform:

1. Self-sign up - As a DTC company Origin allows users to self-sign up to the platform using their email and password.
2. Employer eligibility - As a B2B company Origin allows employers to offer access to Origin’s platform via employer benefits, employers must send Origin a file containing all the users enrolled in Origin benefits.

## Getting Started

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/user/repository
   ```
2. Navigate to the project directory:
   ```bash
   cd origin
   ```
3. Run the application:
   ```bash
   docker-compose up --build
   ```
4. Swagger
   ```bash
   https://localhost/swagger/index.html
   ```

### User Access Management Service have the following REST APIs:

**Signup API:**

```jsx
POST (api/SignUp)
{
  "email: "string",
  "password": "string",
  "country": "string/alpha-2/required",
  "fullName": "string",
  "birthDate": "2024-07-01",
  "salary": 0
}

curl -X 'POST' \
  'https://localhost/api/SignUp' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "string",
  "password": "string",
  "country": "string",
  "fullName": "string",
  "birthDate": "2024-07-01",
  "salary": 0
}'
```

The signup API receives the user email and password, its flow is:

1. Check if the email is associated with some employer via the eligibility file
    1. If it is, use employer-provided data to create the user
    2. If not, validate if the email already exists
2. Validate password strength
    1. Minimum 8 characters, letters, symbols, and numbers
3. Create the user on User Service

**List Users associeted with an Employer:**

```jsx
GET (api/SignUp/{employerName})
{
  "success": true,
  "message": "string",
  "users": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "active": true,
      "createdAt": "2024-07-01",
      "updatedAt": "2024-07-01",
      "email": "string",
      "country": "string",
      "fullName": "string",
      "birthDate": "2024-07-01",
      "salary": 0
    }
  ]
}

curl -X 'GET' \
  'https://localhost/api/SignUp/employerName' \
  -H 'accept: text/plain'
```

The endpoint returns all users associated with the given employer.
The purpose of this endpoint is data visualization.

**Eligibility file API:**

```jsx
POST (api/EligibilityFile)
{
  "file": "url to some blob storage file"
  "employer_name": "string"
}

curl -X 'POST' \
  'https://localhost/api/EligibilityFile' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "file": "string",
  "employer_name": "string"
}'
```

This API receives the URL to a CSV file to be downloaded and processed, CSV file has the following columns:

| Column | Description | Required |
| --- | --- | --- |
| email | The email of the employee | True |
| full_name | The name of the employee | False |
| country | The country of the employee | True |
| birth_date | Birth date of the employee | False |
| salary | The user salary | False |


https://ildjfbd.blob.core.windows.net/csv/10_employees.csv
https://ildjfbd.blob.core.windows.net/csv/10_employees_without_user4.csv
https://ildjfbd.blob.core.windows.net/csv/100_employees.csv
https://ildjfbd.blob.core.windows.net/csv/1k_employees.csv

If the informed employer does not exists, It will be created.

The file will be enqueued and will be processed line-by-line, checking for the required columns on each line in background.
If already exists a file being processed, it will not be enqueued. You must wait.

During the file processing, it will:

- Check if the user already exists and update the country and salary
- Terminate the accounts of users attached to that employer that are no longer coming in the eligibility file (this means that the user left his current employer)

**Get Active Eligibility file:**

```jsx
GET (api/EligibilityFile/{employerName})
{
  "success": true,
  "message": "string",
  "elibilityFile": {
    "id": 0,
    "active": true,
    "createdAt": "2024-07-01",
    "updatedAt": "2024-07-01",
    "url": "string",
    "employerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "status": "string: Processing | Processed | Error",
    "message": "string"
  }
}

curl -X 'GET' \
  'https://localhost/api/EligibilityFile/employerName' \
  -H 'accept: text/plain'
```
Returns the last eligibility file created for the given employer.
As the file processing is done in backgroud, this endpoint is available to get the current status of the file.

** Get file processing report**

```jsx
GET (api/EligibilityFile/report/{employerName})

curl -X 'GET' \
  'https://localhost/api/EligibilityFile/report/employerName' \
  -H 'accept: text/plain'
```
Returns the report file for the last active eligibility file processed with success for the given employer.

**User Service**

 Assuming the User Service have these endpoints:
 
```jsx
POST /users
{
  "email": "string/required",
  "password": "string/required"
  "country": "string/alpha-2/required",
  "access_type": "string: dtc|employer / required"
  "full_name": "string/optional",
  "employer_id": "string/optional",
  "birth_date": "date/optional",
  "salary": "decimal/optional"
}
```

```jsx
GET /users?email=value
{
  "id": "string",
  "email": "string/required",
  "country": "string/alpha-2/required",
  "access_type": "string: dtc|employer / required",
  "full_name": "string/optional",
  "employer_id": "string/optional",
  "birth_date": "date/optional",
  "salary": "decimal/optional"
}
```

```jsx
GET /users/{id}
{
  "id": "string",
  "email": "string/required",
  "country": "string/alpha-2/required",
  "access_type": "string: dtc|employer / required",
  "full_name": "string/optional",
  "employer_id": "string/optional",
  "birth_date": "date/optional",
  "salary": "decimal/optional"
}
```

```
GET /users/employer/{employerId}
{
  [
    {
	  "id": "string",
	  "email": "string/required",
	  "country": "string/alpha-2/required",
	  "access_type": "string: dtc|employer / required",
	  "full_name": "string/optional",
	  "employer_id": "string/optional",
	  "birth_date": "date/optional",
	  "salary": "decimal/optional"
    }
  ]
}
```

```jsx
PATCH /users/{id}
[
  {
    "field": "country",
    "value": "US"
  }
]
```

```jsx
DELETE /users/{id}

```

**Employer Service**

 Assuming the Employer Service have these endpoints:

```jsx
POST /employers
{
  "name": "string"
}
```

```jsx
GET /employers?name=value
{
  "id": "string",
  "name": "string"
}
```

```jsx
GET /employers/{id}
{
  "id": "string",
  "name": "string"
}
```

