\c job_management

-- FUNCTION: public.get_machine_life_history(uuid)

-- DROP FUNCTION public.get_machine_life_history(uuid);

CREATE OR REPLACE FUNCTION public.get_machine_life_history(
	p_instance_id uuid)
	RETURNS machine_life_history
	LANGUAGE 'sql'

AS $BODY$
	SELECT *
	FROM machine_life_history
	WHERE instance_id = COALESCE(p_instance_id, instance_id)
$BODY$;

ALTER FUNCTION public.get_machine_life_history(uuid)
	OWNER TO postgres;

-- FUNCTION: public.delete_machine_life_history(uuid)

-- DROP FUNCTION public.delete_machine_life_history(uuid);

CREATE OR REPLACE FUNCTION public.delete_machine_life_history(
	p_instance_id uuid)
	RETURNS void
	LANGUAGE 'sql'
	
AS $BODY$
	DELETE FROM public.machine_life_history
	WHERE instance_id = p_instance_id
$BODY$;

ALTER FUNCTION public.delete_machine_life_history(uuid)
	OWNER TO postgres;


-- FUNCTION: public.insert_machine_life_history(uuid, uuid, character varying, text, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone)

-- DROP FUNCTION public.insert_machine_life_history(uuid, uuid, character varying, text, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone);

CREATE OR REPLACE FUNCTION public.insert_machine_life_history(
	p_instance_id uuid,
	p_host_name character varying,
	p_instance_type text,
	p_creation_time timestamp with time zone,
	p_test_start_time timestamp with time zone,
	p_test_end_time timestamp with time zone,
	p_benchmarks json,
	p_destruction_time timestamp with time zone)
	RETURNS uuid
	LANGUAGE 'sql'
	
AS $BODY$
	INSERT INTO public.machine_life_history(
		id, instance_id, host_name, instance_type, 
		creation_time, test_start_time, test_end_time, 
		benchmarks, destruction_time)
	VALUES (uuid_generate_v4(), p_instance_id, p_host_name, p_instance_type, 
			p_creation_time, p_test_start_time, p_test_end_time,
			p_benchmarks, p_destruction_time)
	RETURNING id;
$BODY$;

ALTER FUNCTION public.insert_machine_life_history(uuid, character varying, text, timestamp with time zone, timestamp with time zone, timestamp with time zone, json, timestamp with time zone)
	OWNER TO postgres;
