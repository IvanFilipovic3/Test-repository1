-- Database: data_modelling

-- DROP DATABASE data_modelling;

CREATE DATABASE data_modelling
	WITH 
	OWNER = postgres
	ENCODING = 'UTF8'
	LC_COLLATE = 'English_United Kingdom.1252'
	LC_CTYPE = 'English_United Kingdom.1252'
	TABLESPACE = pg_default
	CONNECTION LIMIT = -1
	TEMPLATE template0;

\c data_modelling

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Table: public.job_templates

-- DROP TABLE public.job_templates;

CREATE TABLE IF NOT EXISTS public.job_templates
(
	id uuid NOT NULL,
	name character varying(100) COLLATE pg_catalog."default" NOT NULL,
	description character varying(1000) COLLATE pg_catalog."default" NOT NULL,
	workspace_id uuid NOT NULL,
	run_setting_id uuid NOT NULL,
	structure_id uuid NOT NULL,
	priority integer NOT NULL,
	min_machine_units integer NOT NULL,
	max_machine_units integer NOT NULL,
	unit_type smallint NOT NULL,
	min_worker_memory integer NOT NULL,
	instance_type text COLLATE pg_catalog."default" NOT NULL,
	ce_version character varying(50) COLLATE pg_catalog."default" NOT NULL,
	plugin_version character varying(50) COLLATE pg_catalog."default" NOT NULL,
	compiler character varying(100) COLLATE pg_catalog."default" NOT NULL,
	enable_avx boolean NOT NULL,
	enable_gpu boolean NOT NULL,
	sims_per_task integer NOT NULL,
	mp_batch_size integer NOT NULL,
	enable_pmpp boolean NOT NULL,
	enable_tmpo boolean NOT NULL,
	split_type integer NOT NULL,
	split_nested_structs boolean NOT NULL,
	calculated_variables text[] COLLATE pg_catalog."default" NOT NULL,
	result_definitions text[] COLLATE pg_catalog."default" NOT NULL,
	start_date date NOT NULL,
	future_accum_period integer NOT NULL,
	past_accum_period integer NOT NULL,
	company_year_end integer NOT NULL,
	dynamic_proj_period integer NOT NULL,
	run_numbers text COLLATE pg_catalog."default" NOT NULL,
	simulations text COLLATE pg_catalog."default" NOT NULL,
	produce_xps_runlog boolean NOT NULL,
	CONSTRAINT job_templates_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public.job_templates
	OWNER to postgres;
