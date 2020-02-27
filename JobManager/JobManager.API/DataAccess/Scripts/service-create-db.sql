-- Database: job_management

-- DROP DATABASE job_management;

CREATE DATABASE job_management
	WITH 
	OWNER = postgres
	ENCODING = 'UTF8'
	LC_COLLATE = 'English_United Kingdom.1252'
	LC_CTYPE = 'English_United Kingdom.1252'
	TABLESPACE = pg_default
	CONNECTION LIMIT = -1
	TEMPLATE template0;

\c job_management

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Table: public.job_queue

-- DROP TABLE public.job_queue;

CREATE TABLE IF NOT EXISTS public.job_queue
(
	id uuid NOT NULL,
	name character varying(100) COLLATE pg_catalog."default" NOT NULL,
	client_reference text COLLATE pg_catalog."default" NOT NULL,
	callback_url text COLLATE pg_catalog."default" NOT NULL,
	subtmitted_time timestamp without time zone NOT NULL,
	priority integer NOT NULL,
	job_type smallint NOT NULL,
	expected_duration interval NOT NULL,
	maximum_duration interval NOT NULL,
	job_details json NOT NULL,
	min_machine_units integer NOT NULL,
	max_machine_units integer NOT NULL,
	unit_type smallint NOT NULL,
	min_worker_memory integer NOT NULL,
	instance_type text COLLATE pg_catalog."default" NOT NULL,
	run_start_time timestamp without time zone,
	number_of_tasks integer NOT NULL DEFAULT 0,
	tasks_completed integer NOT NULL DEFAULT 0,
	tasks_failed integer NOT NULL DEFAULT 0,
	CONSTRAINT job_queue_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public.job_queue
	OWNER to postgres;

-- Table: public.machine_activity

-- DROP TABLE public.machine_activity;

CREATE TABLE IF NOT EXISTS public.machine_activity
(
	instance_id uuid NOT NULL,
	host_name character varying(100) COLLATE pg_catalog."default" NOT NULL,
	instance_type text COLLATE pg_catalog."default" NOT NULL,
	job_id uuid,
	status smallint NOT NULL,
	role smallint,
	creation_time timestamp without time zone NOT NULL,
	test_start_time timestamp without time zone,
	test_end_time timestamp without time zone,
	benchmarks json,
	run_join_time timestamp without time zone,
	run_leave_time timestamp without time zone,
	status_detail json,
	CONSTRAINT machine_activity_pkey PRIMARY KEY (instance_id)
)

TABLESPACE pg_default;

ALTER TABLE public.machine_activity
	OWNER to postgres;

-- Table: public.machine_history

-- DROP TABLE public.machine_history;

CREATE TABLE IF NOT EXISTS public.machine_history
(
	id uuid NOT NULL,
	instance_id uuid NOT NULL,
	host_name character varying(100) COLLATE pg_catalog."default" NOT NULL,
	instance_type text COLLATE pg_catalog."default" NOT NULL,
	job_id uuid NOT NULL,
	role smallint NOT NULL,
	run_join_time timestamp without time zone NOT NULL,
	run_leave_time timestamp without time zone NOT NULL,
	status_detail json,
	CONSTRAINT machine_history_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public.machine_history
	OWNER to postgres;

-- Table: public.machine_life_history

-- DROP TABLE public.machine_life_history;

CREATE TABLE IF NOT EXISTS public.machine_life_history
(
	id uuid NOT NULL,
	instance_id uuid NOT NULL,
	host_name character varying(100) COLLATE pg_catalog."default" NOT NULL,
	instance_type text COLLATE pg_catalog."default" NOT NULL,
	creation_time timestamp without time zone NOT NULL,
	test_start_time timestamp without time zone,
	test_end_time timestamp without time zone,
	benchmarks json,
	destruction_time timestamp without time zone,
	CONSTRAINT machine_life_history_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public.machine_life_history
	OWNER to postgres;
