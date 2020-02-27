\c job_management

-- FUNCTION: public.get_job_queue(uuid)

-- DROP FUNCTION public.get_job_queue(uuid);

CREATE OR REPLACE FUNCTION public.get_job_queue(
	p_id uuid)
	RETURNS job_queue
	LANGUAGE 'sql'

AS $BODY$
	SELECT *
	FROM job_queue
	WHERE id = COALESCE(p_id, id)
$BODY$;

ALTER FUNCTION public.get_job_queue(uuid)
	OWNER TO postgres;

-- FUNCTION: public.delete_job_queue(uuid)

-- DROP FUNCTION public.delete_job_queue(uuid);

CREATE OR REPLACE FUNCTION public.delete_job_queue(
	p_id uuid)
	RETURNS void
	LANGUAGE 'sql'

AS $BODY$
	DELETE FROM job_queue
	WHERE id = p_id
$BODY$;

ALTER FUNCTION public.delete_job_queue(uuid)
	OWNER TO postgres;

-- FUNCTION: public.insert_job_queue(uuid, character varying, text, text, timestamp with time zone, integer, smallint, interval, interval, json, integer, integer, smallint, integer, text, timestamp with time zone, integer, integer, integer)

-- DROP FUNCTION public.insert_job_queue(uuid, character varying, text, text, timestamp with time zone, integer, smallint, interval, interval, json, integer, integer, smallint, integer, text, timestamp with time zone, integer, integer, integer);

CREATE OR REPLACE FUNCTION public.insert_job_queue(
	p_name character varying,
	p_client_reference text,
	p_callback_url text,
	p_subtmitted_time timestamp with time zone,
	p_priority integer,
	p_job_type smallint,
	p_expected_duration interval,
	p_maximum_duration interval,
	p_job_details json,
	p_min_machine_units integer,
	p_max_machine_units integer,
	p_unit_type smallint,
	p_min_worker_memory integer,
	p_instance_type text,
	p_run_start_time timestamp with time zone,
	p_number_of_tasks integer,
	p_tasks_completed integer,
	p_tasks_failed integer)
	RETURNS uuid
	LANGUAGE 'sql'
	
AS $BODY$
	INSERT INTO public.job_queue(
		id, name, client_reference, callback_url, 
		subtmitted_time, priority, job_type, expected_duration, 
		maximum_duration, job_details, min_machine_units, 
		max_machine_units, unit_type, min_worker_memory, 
		instance_type, run_start_time, 
		number_of_tasks, tasks_completed, tasks_failed)
	VALUES (uuid_generate_v4(), p_name, p_client_reference, p_callback_url, 
			p_subtmitted_time, p_priority, p_job_type, p_expected_duration,
			p_maximum_duration, p_job_details, p_min_machine_units, 
			p_max_machine_units, p_unit_type, p_min_worker_memory, 
			p_instance_type, p_run_start_time, 
			p_number_of_tasks, p_tasks_completed,p_tasks_failed)
	RETURNING id;
$BODY$;

ALTER FUNCTION public.insert_job_queue(character varying, text, text, timestamp with time zone, integer, smallint, interval, interval, json, integer, integer, smallint, integer, text, timestamp with time zone, integer, integer, integer)
	OWNER TO postgres;

-- FUNCTION: public."update-job-queue"(uuid, character varying, text, text, timestamp with time zone, integer, smallint, interval, interval, json, integer, integer, smallint, integer, text, timestamp with time zone, integer, integer, integer)

-- DROP FUNCTION public."update-job-queue"(uuid, character varying, text, text, timestamp with time zone, integer, smallint, interval, interval, json, integer, integer, smallint, integer, text, timestamp with time zone, integer, integer, integer);

CREATE OR REPLACE FUNCTION public."update-job-queue"(
	p_id uuid,
	p_name character varying,
	p_client_reference text,
	p_callback_url text,
	p_subtmitted_time timestamp with time zone,
	p_priority integer,
	p_job_type smallint,
	p_expected_duration interval,
	p_maximum_duration interval,
	p_job_details json,
	p_min_machine_units integer,
	p_max_machine_units integer,
	p_unit_type smallint,
	p_min_worker_memory integer,
	p_instance_type text,
	p_run_start_time timestamp with time zone,
	p_number_of_tasks integer,
	p_tasks_completed integer,
	p_tasks_failed integer)
	RETURNS void
	LANGUAGE 'sql'

AS $BODY$
	UPDATE public.job_queue
		SET name=p_name, client_reference=p_client_reference, callback_url=p_callback_url, 
		subtmitted_time=p_subtmitted_time, priority=p_priority, job_type=p_job_type, 
		expected_duration=p_expected_duration, maximum_duration=p_maximum_duration, job_details=p_job_details,
		min_machine_units=p_min_machine_units, max_machine_units=p_max_machine_units, unit_type=p_unit_type, 
		min_worker_memory=p_min_worker_memory, instance_type=p_instance_type, 
		run_start_time=p_run_start_time, number_of_tasks=p_number_of_tasks, tasks_completed=p_tasks_completed, 
		tasks_failed=p_tasks_failed
	WHERE id = p_id
$BODY$;

ALTER FUNCTION public."update-job-queue"(uuid, character varying, text, text, timestamp with time zone, integer, smallint, interval, interval, json, integer, integer, smallint, integer, text, timestamp with time zone, integer, integer, integer)
	OWNER TO postgres;
