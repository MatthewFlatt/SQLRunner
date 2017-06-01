# SQLRunner
Static data runner for Redgate DLM Automation - console app.

Inserts the static data from a SQL Source Control repositoy directly into an empty database.

This can be used to speed up DLM Automation builds when large amounts of static data are involved.

Will work out dependency order using foreign keys.

Command line arguments:
-S SQL Server instance to connect to
-D SQL Server database to insert rows to
-F Folder which contains static data SQL files
