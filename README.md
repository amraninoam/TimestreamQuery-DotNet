# Timestream Query C# Application

This is a C# application that queries an AWS Timestream database and prints the results.

## Prerequisites

- .NET Core SDK (version 3.1 or later)
- AWS CLI configured with the appropriate profile
- Docker (optional, for containerized deployment)

## Setup

1. Clone the repository:

    ```sh
    git clone https://github.com/amraninoam/TimestreamQuery-DotNet
    cd TimestreamQuery-DotNet
    ```

2. Install dependencies:

    ```sh
    dotnet restore
    ```

3. Create a `.env` file in the root of the project and add the following environment variables:

    ```plaintext
    dbName=your_db_name
    tableName=your_table_name
    profile=your_aws_profile
    rows=number_of_rows
    serviceURL=your_timestream_service_url
    ```

## Running the Application

To run the application:

```sh
dotnet run
