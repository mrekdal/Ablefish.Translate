CREATE SCHEMA [Web]
    AUTHORIZATION [dbo];




GO
GRANT EXECUTE
    ON SCHEMA::[Web] TO [WebPublicUser];


GO
GRANT EXECUTE
    ON SCHEMA::[Web] TO [WebAdminUser];


GO
GRANT SELECT
    ON SCHEMA::[Web] TO [WebPublicUser];

