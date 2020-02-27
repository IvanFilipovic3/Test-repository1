\c job_management

-- FUNCTION: public.get_machine_history(uuid)

-- DROP FUNCTION public.get_machine_history(uuid);

CREATE OR REPLACE FUNCTION public.get_machine_history(
	p_instance_id uuid)
	RETURNS machine_history
	LANGUAGE 'sql'
	
AS $BODY$
	SELECT *
	FROM machine_history
	WHERE instance_id = COALESCE(p_instance_id, instance_id)
$BODY$;

ALTER FUNCTION public.get_machine_history(uuid)
	OWNER TO postgres;

-- FUNCTION: public.delete_machine_history(uuid)

-- DROP FUNCTION public.delete_machine_history(uuid);

CREATE OR REPLACE FUNCTION public.delete_machine_history(
	p_instance_id uuid)
	RETURNS void
	LANGUAGE 'sql'

AS $BODY$
	DELETE FROM public.machine_history
	WHERE instance_id = p_instance_id
$BODY$;

ALTER FUNCTION public.delete_machine_history(uuid)
	OWNER TO postgres;

-- FUNCTION: public.insert_machine_history(uuid, uuid, character varying, text, uuid, smallint, timestamp with time zone, timestamp with time zone, json)

-- DROP FUNCTION public.insert_machine_history(uuid, uuid, character varying, text, uuid, smallint, timestamp with time zone, timestamp with time zone, json);

CREATE OR REPLACE FUNCTION public.insert_machine_history(
	p_instance_id uuid,
	p_host_name character varying,
	p_instance_type text,
	p_job_id uuid,
	p_role smallint,
	p_run_join_time timestamp with time zone,
	p_run_leave_time timestamp with time zone,
	p_status_detail json)
	RETURNS uuid
	LANGUAGE 'sql'
	
AS $BODY$
	INSERT INTO public.machine_history(
		id, instance_id, host_name, instance_type, 
		job_id, role, run_join_time, run_leave_time, status_detail)
	VALUES (uuid_generate_v4(), p_instance_id, p_host_name, p_instance_type,
			p_job_id, p_role, p_run_join_time, p_run_leave_time, 
		   p_status_detail)
	RETURNING id;
$BODY$;

ALTER FUNCTION public.insert_machine_history(uuid, character varying, text, uuid, smallint, timestamp with time zone, timestamp with time zone, json)
	OWNER TO postgres;
