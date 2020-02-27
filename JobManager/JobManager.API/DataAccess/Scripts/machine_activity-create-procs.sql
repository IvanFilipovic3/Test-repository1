\c job_management

-- FUNCTION: public.get_machine_activity(uuid)

-- DROP FUNCTION public.get_machine_activity(uuid);

CREATE OR REPLACE FUNCTION public.get_machine_activity(
	p_instance_id uuid)
	RETURNS machine_activity
	LANGUAGE 'sql'

AS $BODY$
	SELECT *
	FROM machine_activity
	WHERE instance_id = COALESCE(p_instance_id, instance_id)
$BODY$;

ALTER FUNCTION public.get_machine_activity(uuid)
	OWNER TO postgres;

-- FUNCTION: public.delete_machine_activity(uuid)

-- DROP FUNCTION public.delete_machine_activity(uuid);

CREATE OR REPLACE FUNCTION public.delete_machine_activity(
	p_instance_id uuid)
	RETURNS void
	LANGUAGE 'sql'
	
AS $BODY$
	DELETE FROM public.machine_activity
	WHERE instance_id = p_instance_id
$BODY$;

ALTER FUNCTION public.delete_machine_activity(uuid)
	OWNER TO postgres;

-- FUNCTION: public.insert_machine_activity(uuid, character varying, text, uuid, smallint, smallint, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone, timestamp with time zone, json)

-- DROP FUNCTION public.insert_machine_activity(uuid, character varying, text, uuid, smallint, smallint, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone, timestamp with time zone, json);

CREATE OR REPLACE FUNCTION public.insert_machine_activity(
	p_instance_id uuid,
	p_host_name character varying,
	p_instance_type text,
	p_job_id uuid,
	p_status smallint,
	p_role smallint,
	p_creation_time timestamp with time zone,
	p_test_start_time timestamp with time zone,
	p_test_end_time timestamp with time zone,
	p_benchmarks json,
	p_run_join_time timestamp with time zone,
	p_run_leave_time timestamp with time zone,
	p_status_detail json)
	RETURNS void
	LANGUAGE 'sql'
	
AS $BODY$
	INSERT INTO public.machine_activity(
		instance_id, host_name, instance_type, 
		job_id, status, role, creation_time, 
		test_start_time, test_end_time, benchmarks, 
		run_join_time, run_leave_time, status_detail)
	VALUES (p_instance_id, p_host_name, p_instance_type, 
			p_job_id, p_status, p_role, p_creation_time, 
			p_test_start_time, p_test_end_time, p_benchmarks, 
			p_run_join_time, p_run_leave_time, p_status_detail);
$BODY$;

ALTER FUNCTION public.insert_machine_activity(uuid, character varying, text, uuid, smallint, smallint, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone, timestamp with time zone, json)
	OWNER TO postgres;

-- FUNCTION: public.update_machine_activity(uuid, character varying, text, uuid, smallint, smallint, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone, timestamp with time zone, json)

-- DROP FUNCTION public.update_machine_activity(uuid, character varying, text, uuid, smallint, smallint, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone, timestamp with time zone, json);

CREATE OR REPLACE FUNCTION public.update_machine_activity(
	p_instance_id uuid,
	p_host_name character varying,
	p_instance_type text,
	p_job_id uuid,
	p_status smallint,
	p_role smallint,
	p_creation_time timestamp with time zone,
	p_test_start_time timestamp with time zone,
	p_test_end_time timestamp with time zone,
	p_benchmarks json,
	p_run_join_time timestamp with time zone,
	p_run_leave_time timestamp with time zone,
	p_status_detail json)
	RETURNS void
	LANGUAGE 'sql'
	
AS $BODY$
	UPDATE public.machine_activity
	SET host_name=p_host_name, instance_type=p_instance_type, 
		job_id=p_job_id, status=p_status, role=p_role, 
		creation_time=p_creation_time, 
		test_start_time=p_test_start_time, test_end_time=p_test_end_time, 
		benchmarks=p_benchmarks, 
		run_join_time=p_run_join_time, run_leave_time=p_run_leave_time, 
		status_detail=p_status_detail
	WHERE instance_id = p_instance_id
$BODY$;

ALTER FUNCTION public.update_machine_activity(uuid, character varying, text, uuid, smallint, smallint, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone, timestamp with time zone, json)
	OWNER TO postgres;
