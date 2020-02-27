\c data_modelling

-- FUNCTION: public.get_job_templates(uuid)

-- DROP FUNCTION public.get_job_templates(uuid);

CREATE OR REPLACE FUNCTION public.get_job_templates(
	p_id uuid)
	RETURNS SETOF job_templates
	LANGUAGE 'sql'

	
AS $BODY$
	SELECT *
	FROM job_templates
	WHERE id = COALESCE(p_id, id)
$BODY$;

ALTER FUNCTION public.get_job_templates(uuid)
	OWNER TO postgres;

-- FUNCTION: public.delete_job_template(uuid)

-- DROP FUNCTION public.delete_job_template(uuid);

CREATE OR REPLACE FUNCTION public.delete_job_template(
	p_id uuid)
	RETURNS void
	LANGUAGE 'sql'
	
AS $BODY$
	DELETE FROM public.job_templates
		WHERE id = p_id
$BODY$;

ALTER FUNCTION public.delete_job_template(uuid)
	OWNER TO postgres;
	
-- FUNCTION: public.insert_job_template(uuid, character varying, character varying, uuid, uuid, uuid, integer, integer, integer, smallint, integer, text, character varying, character varying, character varying, boolean, boolean, integer, integer, boolean, boolean, integer, boolean, text, text, date, integer, integer, integer, integer, text, text, boolean)

-- DROP FUNCTION public.insert_job_template(uuid, character varying, character varying, uuid, uuid, uuid, integer, integer, integer, smallint, integer, text, character varying, character varying, character varying, boolean, boolean, integer, integer, boolean, boolean, integer, boolean, text, text, date, integer, integer, integer, integer, text, text, boolean);

CREATE OR REPLACE FUNCTION public.insert_job_template(
	p_name character varying,
	p_description character varying,
	p_workspace_id uuid,
	p_run_setting_id uuid,
	p_structure_id uuid,
	p_priority integer,
	p_min_machine_units integer,
	p_max_machine_units integer,
	p_unit_type smallint,
	p_min_worker_memory integer,
	p_instance_type text,
	p_ce_version character varying,
	p_plugin_version character varying,
	p_compiler character varying,
	p_enable_avx boolean,
	p_enable_gpu boolean,
	p_sims_per_task integer,
	p_mp_batch_size integer,
	p_enable_pmpp boolean,
	p_enable_tmpo boolean,
	p_split_type integer,
	p_split_nested_structs boolean,
	p_calculated_variables text[],
	p_result_definitions text[],
	p_start_date date,
	p_future_accum_period integer,
	p_past_accum_period integer,
	p_company_year_end integer,
	p_dynamic_proj_period integer,
	p_run_numbers text,
	p_simulations text,
	p_produce_xps_runlog boolean)
	RETURNS uuid
	LANGUAGE 'sql'

	
AS $BODY$
	INSERT INTO public.job_templates(
		id, name, description, workspace_id, run_setting_id, structure_id,
		priority, min_machine_units, max_machine_units, 
		unit_type, min_worker_memory, instance_type,
		ce_version, plugin_version, compiler, 
		enable_avx, enable_gpu, sims_per_task, mp_batch_size, 
		enable_pmpp, enable_tmpo, split_type, split_nested_structs,
		calculated_variables, result_definitions, start_date, 
		future_accum_period, past_accum_period, company_year_end, 
		dynamic_proj_period, run_numbers, simulations, produce_xps_runlog)
		VALUES (uuid_generate_v4(), p_name, p_description, p_workspace_id, p_run_setting_id, p_structure_id,
				p_priority, p_min_machine_units, p_max_machine_units,
				p_unit_type, p_min_worker_memory, p_instance_type, 
				p_ce_version, p_plugin_version, p_compiler, 
				p_enable_avx, p_enable_gpu, p_sims_per_task, p_mp_batch_size, 
				p_enable_pmpp, p_enable_tmpo, p_split_type, p_split_nested_structs,
				p_calculated_variables, p_result_definitions, p_start_date, 
				p_future_accum_period, p_past_accum_period, p_company_year_end,
				p_dynamic_proj_period, p_run_numbers, p_simulations, p_produce_xps_runlog)
		RETURNING id;
$BODY$;

ALTER FUNCTION public.insert_job_template(character varying, character varying, uuid, uuid, uuid, integer, integer, integer, smallint, integer, text, character varying, character varying, character varying, boolean, boolean, integer, integer, boolean, boolean, integer, boolean, text[], text[], date, integer, integer, integer, integer, text, text, boolean)
	OWNER TO postgres;

-- FUNCTION: public.update_job_template(uuid, character varying, character varying, uuid, uuid, uuid, integer, integer, integer, smallint, integer, text, character varying, character varying, character varying, boolean, boolean, integer, integer, boolean, boolean, integer, boolean, text, text, date, integer, integer, integer, integer, text, text, boolean)

-- DROP FUNCTION public.update_job_template(uuid, character varying, character varying, uuid, uuid, uuid, integer, integer, integer, smallint, integer, text, character varying, character varying, character varying, boolean, boolean, integer, integer, boolean, boolean, integer, boolean, text, text, date, integer, integer, integer, integer, text, text, boolean);

CREATE OR REPLACE FUNCTION public.update_job_template(
	p_id uuid,
	p_name character varying,
	p_description character varying,
	p_workspace_id uuid,
	p_run_setting_id uuid,
	p_structure_id uuid,
	p_priority integer,
	p_min_machine_units integer,
	p_max_machine_units integer,
	p_unit_type smallint,
	p_min_worker_memory integer,
	p_instance_type text,
	p_ce_version character varying,
	p_plugin_version character varying,
	p_compiler character varying,
	p_enable_avx boolean,
	p_enable_gpu boolean,
	p_sims_per_task integer,
	p_mp_batch_size integer,
	p_enable_pmpp boolean,
	p_enable_tmpo boolean,
	p_split_type integer,
	p_split_nested_structs boolean,
	p_calculated_variables text[],
	p_result_definitions text[],
	p_start_date date,
	p_future_accum_period integer,
	p_past_accum_period integer,
	p_company_year_end integer,
	p_dynamic_proj_period integer,
	p_run_numbers text,
	p_simulations text,
	p_produce_xps_runlog boolean)
	RETURNS void
	LANGUAGE 'sql'

	
AS $BODY$
	UPDATE public.job_templates
	SET name=p_name, description=p_description, 
		workspace_id=p_workspace_id, run_setting_id=p_run_setting_id, structure_id=p_structure_id,
		priority=p_priority, min_machine_units=p_min_machine_units, max_machine_units=p_max_machine_units, 
		unit_type=p_unit_type, min_worker_memory=p_min_worker_memory, instance_type=p_instance_type,
		ce_version=p_ce_version, plugin_version=p_plugin_version, compiler=p_compiler, 
		enable_avx=p_enable_avx, enable_gpu=p_enable_gpu, sims_per_task=p_sims_per_task, mp_batch_size=p_mp_batch_size, 
		enable_pmpp=p_enable_pmpp, enable_tmpo=p_enable_tmpo, split_type=p_split_type, split_nested_structs=p_split_nested_structs,
		calculated_variables=p_calculated_variables, result_definitions=p_result_definitions, start_date=p_start_date, 
		future_accum_period=p_future_accum_period, past_accum_period=p_past_accum_period, company_year_end=p_company_year_end, 
		dynamic_proj_period=p_dynamic_proj_period, run_numbers=p_run_numbers, simulations=p_simulations, produce_xps_runlog=p_produce_xps_runlog
	WHERE id=p_id
$BODY$;

ALTER FUNCTION public.update_job_template(uuid, character varying, character varying, uuid, uuid, uuid, integer, integer, integer, smallint, integer, text, character varying, character varying, character varying, boolean, boolean, integer, integer, boolean, boolean, integer, boolean, text[], text[], date, integer, integer, integer, integer, text, text, boolean)
	OWNER TO postgres;
