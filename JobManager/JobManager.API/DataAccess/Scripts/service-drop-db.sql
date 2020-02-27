-- Database: job_management

-- terminate the active connections
SELECT
   pg_terminate_backend(pid)
FROM
   pg_stat_activity
WHERE
   datname = 'job_management';

-- Drop the database
SET client_min_messages = error;
DROP DATABASE IF EXISTS job_management;
