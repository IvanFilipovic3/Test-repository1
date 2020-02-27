-- Database: data_modelling

-- terminate the active connections
SELECT
   pg_terminate_backend(pid)
FROM
   pg_stat_activity
WHERE
   datname = 'data_modelling';

-- Drop the database
SET client_min_messages = error;
DROP DATABASE IF EXISTS data_modelling;
