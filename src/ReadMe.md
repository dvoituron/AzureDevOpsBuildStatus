# Azure DevOps Project Status

Use this CLI tool to display all Projects Build Status.

```
State       Project Name     | Build Name        | Version          | Started by
---------   ---------------- | ----------------- | ---------------- | ---------------
SUCCEEDED   MyProject        | Website - CI      | 20191127.1       | DUPONT Cédric
IN PROGRESS MyProject        | Referentiel - ... | 0.2.19149.10     | VOITURON Denis
FAILED      MyProject        | Framework.Data... | 19.10.17.1       | SMITH Gregor...
```

## How to install?

Run this command:

```
dotnet tool install -g Dvoituron.DevOps.ProjectStatus
```

## How to use?

Fist, you need to go to https://dev.azure.com, to generate a **Security Personal Access Token (PAT)**.
In Azure DevOps website, go to Your profile / Personal Access Token (in top of the screen)
and create a new token with 'Build Read' and 'Release Read' authorizations.

Copy the generated token (PAT) to use in this command.

```
az-devops-status --organization=[organisation_name] pat=[generated_pat]
```

The **organisation_name** is the name just after the url dev.azure.com. Example: https://dev.azure.com/organisation_name.

## Road map

The next release will integrate the display of deployment status (Release Status).