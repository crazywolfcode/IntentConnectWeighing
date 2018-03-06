using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentConnectWeighing
{

	 /// <summary>
	 ///数据库中的所有表名称
	 /// </summary>

	  public enum DataTabeName{
	 	 address,
	 	 bank_card,
	 	 bill_image,
	 	 camera_info,
	 	 car_header,
	 	 car_info,
	 	 company,
	 	 config,
	 	 driving_license,
	 	 material,
	 	 material_category,
	 	 people_info,
	 	 scale,
	 	 sync_info,
	 	 user,
	 	 vchicle_license,
	 	 weighing_bill,
	 }

	 public enum AddressEnum{
	 	 id,
	 	 company_id,
	 	 company,
	 	 province_id,
	 	 province,
	 	 city_id,
	 	 city,
	 	 country_id,
	 	 country,
	 	 detail,
	 	 addtime,
	 	 add_user_id,
	 	 add_user_name,
	 	 status,
	 	 is_delete,
	 	 delete_time,
	 	 sync_time,
	 }


	 public enum BankCardEnum{
	 	 id,
	 	 affiliated_user_id,
	 	 affiliated_user_name,
	 	 brank_name,
	 	 address,
	 	 number,
	 	 type,
	 }


	 public enum BillImageEnum{
	 	 id,
	 	 positon,
	 	 address,
	 	 bill_id,
	 	 bill_number,
	 	 type,
	 	 is_up,
	 	 up_time,
	 	 is_delete,
	 	 delete_time,
	 }


	 public enum CameraInfoEnum{
	 	 id,
	 	 ip,
	 	 port,
	 	 user_name,
	 	 password,
	 	 position,
	 	 scale_id,
	 	 client_id,
	 	 company_id,
	 	 status,
	 	 sync_time,
	 	 is_delete,
	 	 delete_time,
	 }


	 public enum CarHeaderEnum{
	 	 id,
	 	 content,
	 	 sync_time,
	 	 is_delete,
	 }


	 public enum CarInfoEnum{
	 	 id,
	 	 car_number,
	 	 driver,
	 	 driver_mobile,
	 	 driver_idnumber,
	 	 owner_id,
	 	 owner_name,
	 	 addtime,
	 	 add_user_id,
	 	 add_user_name,
	 	 update_time,
	 	 update_user_id,
	 	 update_user_name,
	 	 is_delete,
	 	 delete_time,
	 	 sync_time,
	 	 vehicle_id,
	 	 status,
	 	 remark,
	 }


	 public enum CompanyEnum{
	 	 id,
	 	 name,
	 	 legal_person,
	 	 name_first_case,
	 	 credit_number,
	 	 businese_lincense_number,
	 	 licese_esprise_time,
	 	 abbreviation,
	 	 abbreviation_first_case,
	 	 level,
	 	 parent_id,
	 	 remark,
	 	 addtime,
	 	 type,
	 	 status,
	 	 sync_time,
	 	 is_delete,
	 	 address,
	 	 delete_time,
	 }


	 public enum ConfigEnum{
	 	 id,
	 	 client_id,
	 	 client_number,
	 	 config_name,
	 	 config_value,
	 	 config_type,
	 	 description,
	 	 addtime,
	 	 add_user_id,
	 	 add_user_name,
	 	 update_time,
	 	 update_user_id,
	 	 update_user_name,
	 	 sync_time,
	 	 is_delete,
	 	 delete_time,
	 	 status,
	 }


	 public enum DrivingLicenseEnum{
	 	 id,
	 	 name,
	 	 number,
	 	 sex,
	 	 nationality,
	 	 address,
	 	 birthday,
	 	 first_time,
	 	 driver_class,
	 	 local_img,
	 	 remote_img,
	 	 effict_year,
	 	 start_time,
	 	 end_time,
	 	 affiliated_user_id,
	 	 is_delete,
	 	 delete_time,
	 	 sync_time,
	 	 status,
	 }


	 public enum MaterialEnum{
	 	 id,
	 	 name,
	 	 category_id,
	 	 category_name,
	 	 name_first_case,
	 	 description,
	 	 addtime,
	 	 add_user_id,
	 	 add_user_name,
	 	 update_time,
	 	 update_user_id,
	 	 update_user_name,
	 	 is_delete,
	 	 delete_time,
	 	 sync_time,
	 	 status,
	 }


	 public enum MaterialCategoryEnum{
	 	 id,
	 	 name,
	 	 first_case,
	 	 children_count,
	 	 addtime,
	 	 add_user_id,
	 	 add_user_name,
	 	 add_user_company,
	 	 is_delete,
	 	 delete_time,
	 	 sync_time,
	 }


	 public enum PeopleInfoEnum{
	 	 id,
	 	 name,
	 	 people_name,
	 }


	 public enum ScaleEnum{
	 	 id,
	 	 name,
	 	 com,
	 	 baud_rate,
	 	 data_byte,
	 	 end_byte,
	 	 brand,
	 	 series,
	 	 client_id,
	 	 company_id,
	 	 status,
	 	 add_time,
	 	 add_user_id,
	 	 add_user_name,
	 	 is_default,
	 	 is_delete,
	 	 delete_time,
	 	 sync_time,
	 }


	 public enum SyncInfoEnum{
	 	 id,
	 	 client_id,
	 	 client_number,
	 	 table_name,
	 	 sync_time,
	 	 last_update_time,
	 }


	 public enum UserEnum{
	 	 id,
	 	 name,
	 	 sex,
	 	 nation,
	 	 login_name,
	 	 passwprd,
	 	 weichat,
	 	 qq,
	 	 email,
	 	 mobilephone,
	 	 header_img_url,
	 	 birth_date,
	 	 id_number,
	 	 role_level,
	 	 addtime,
	 	 add_user_id,
	 	 add_user_name,
	 	 affiliated_company_id,
	 	 company,
	 	 is_delete,
	 	 delete_time,
	 	 sync_time,
	 	 address,
	 	 user_type,
	 	 status,
	 	 remark,
	 }


	 public enum VchicleLicenseEnum{
	 	 id,
	 	 car_number,
	 	 car_type,
	 	 owner,
	 	 address,
	 	 use_character,
	 	 brand,
	 	 vin,
	 	 engine_number,
	 	 register_date,
	 	 issue_date,
	 	 affiliated_car_id,
	 	 is_delete,
	 	 delete_time,
	 	 sync_time,
	 	 status,
	 }


	 public enum WeighingBillEnum{
	 	 id,
	 	 type,
	 	 relative_bill_id,
	 	 send_number,
	 	 send_company_id,
	 	 send_company_name,
	 	 send_in_time,
	 	 send_out_time,
	 	 send_user_id,
	 	 send_user_name,
	 	 send_gross_weight,
	 	 send_trae_weight,
	 	 send_net_weight,
	 	 send_material_id,
	 	 send_material_name,
	 	 send_remark,
	 	 receive_number,
	 	 receive_company_id,
	 	 receive_company_name,
	 	 receive_in_time,
	 	 receive_out_time,
	 	 receive_user_id,
	 	 receive_user_name,
	 	 receive_gross_weight,
	 	 receive_trae_weight,
	 	 receive_material_id,
	 	 receive_material_name,
	 	 receive_remark,
	 	 receive_net_weight,
	 	 difference_weight,
	 	 deducation_weiht,
	 	 deducation_description,
	 	 car_id,
	 	 plate_number,
	 	 driver,
	 	 driver_mobile,
	 	 driver_id_number,
	 	 print_date_time,
	 	 print_times,
	 	 up_status,
	 	 sync_time,
	 	 affiliated_company_id,
	 	 is_delete,
	 	 delete_time,
	 }

}
