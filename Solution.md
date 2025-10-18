

# DB
## Migrations
Migrations are stored in the ``packages/BookStore.ApiService/src/Database/Migrations`` folder
They can be applied in many ways
- During dev the main app can run them at startup
- In prod the best solution is to make a small migrator app that is run as a pre deploy container in k8s

# Auth
## Auth
Currently with ``DummyAuthenticationHandler`` we accept any header that claims to be user, than load the roles and tenant from the db
This will need to be replaced by Azure Entra or similar.

## Authorisation
Authorisation is done by creating policies
One custom policy ``TenantAccessAuthorization`` this checks that the user claim's tenant is the same as the tenant they are trying to access

