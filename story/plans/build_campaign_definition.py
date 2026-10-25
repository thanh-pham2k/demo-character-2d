"""Author ordered production gameplay events; never auto-completes a mission.
Labels/bridges come from the approved45mission baseline. Runtime scene adapters
must emit these events only after their physical/minigame action succeeds.
"""
import json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
plan=json.loads((ROOT/'story/plans/ASSET_BATCH_PLAN.json').read_text())
# id|player-facing action; every item is an explicit gameplay success event.
rows=[
('Station','decline_tout|Từ chối lời mời xe;buy_ticket|Mua vé xe đến KTX;arrive_dorm|Xuống xe tại sảnh KTX'),
('Reception','present_documents|Nộp giấy nhập học;receive_room_key|Nhận chìa khóa404;reach_room|Lên tầng4'),
('Room','unlock_room|Mở phòng404;claim_bed|Chọn giường;place_backpack|Đặt balo;repair_fan|Siết ốc và cắm quạt'),
('Room','buy_cleaning_kit|Mua bộ vệ sinh;clean_four_spots|Lau đủ4điểm;meet_roommates|Làm quen ba bạn'),
('Canteen','choose_meal|Chọn cơm trong ngân sách;eat_together|Ăn và trò chuyện;receive_schedule|Xem lịch học;return_sleep|Về phòng ngủ'),
('H6','equip_backpack|Đeo balo;wash_before_class|Vệ sinh buổi sáng;take_bus|Lên xe đi học;enter_class|Vào lớpH6;solve_warmup|Giải câu khởi động55;finish_lesson|Học hết tiết;return_room|Đi xe về404;tell_dam|Kể chuyện cho Đạm'),
('H6','join_wc_queue|Xếp hàngWC;catch_bus|Đón xe hoặc chuyến sau;attend_class|Điểm danh;finish_year_exams|Hoàn thành các kỳ thi;read_year_results|Xem32tín chỉ'),
('Room','get_cooker|Lấy bếp và nồi;plug_cooker|Cắm bếp;add_ingredients|Thêm đồ ăn;share_feast|Ăn lẩu cùng phòng;toast|Cụng ly;sleep_new_term|Ngủ và chuyển học kỳ3'),
('Room','refresh_registration|Chờ mở đăng ký;select_classes|Chọn lớp;confirm_schedule|Xác nhận lịch06:45'),
('Room','choose_night|Chơi thêm hoặc đi ngủ;end_night|Kết thúc đêm theo lựa chọn'),
('Room','choose_alarm|Dậy hoặc Snooze;show_morning_consequence|Xem hậu quả buổi sáng'),
('Room','hear_guard|Nghe chìa khóa bảo vệ;unplug_cooker|Rút điện;hide_pot|Cất nồi;pretend_sleep|Giả ngủ;resolve_inspection|Kết thúc kiểm tra'),
('Room','compile_assignment|Compile bàiC++;find_error|Tìm lỗi;patch_assignment|Sửa lỗi;test_assignment|Chạy test;submit_assignment|Nộp trước hạn'),
('H6','arrive_exam|Đến phòng thi;answer_known_sections|Làm phần kiến thức đã học;submit_exam|Nộp bài'),
('Room','open_grade_portal|Mở bảng điểm;read_failed_courses|Xem3mônF;read_prerequisite_graph|Xem các môn bị khóa;open_retaking_invoice|Mở hóa đơn học lại'),
('Hallway','answer_mother|Trả lời mẹ;finish_call|Kết thúc cuộc gọi;receive_first_job|Nhận tin khách đầu tiên'),
('Room','receive_laptop|Nhận máy;backup_data|Sao lưu giả lập;boot_usb|Khởi độngUSB;install_system|Cài hệ điều hành;handover_laptop|Bàn giao và nhận50k'),
('Room','post_service_sign|Treo biển;accept_next_jobs|Nhận đơn tiếp theo'),
('Room','complete_job_a|Hoàn thành ca sửa máy1;rest_and_eat|Ăn và nghỉ;complete_job_b|Hoàn thành ca sửa máy2;review_weekly_ledger|Xem thu chi nhiều tuần;pay_retaking_fee|Đóng học phí;pass_programming|PassKTLT;pass_physics|PassVL1;enroll_data_structures|MởCTDL học kỳ6'),
('Room','receive_crush_laptop|Nhận máy của bạn nữ;scan_virus|Quét virus;repair_crush_laptop|Sửa lỗi;waive_labor|Miễn tiền công;receive_cafe_invite|Nhận tin hẹn'),
('Cafe','borrow_jacket|Mượn áo Long;arrive_cafe|Đến quán;meet_boyfriend|Gặp bạn trai của crush;repair_second_laptop|Sửa máy theo checklist;return_jacket|Về trả áo;accept_lake_invite|Nhận lời ra HồĐá'),
('Lake','sit_with_friends|Ngồi cùng nhóm;share_tea|Chia trà và đồ ăn;finish_conversation|Trò chuyện;return_sleep|Về ngủ'),
('Room','open_year3_results|Mở điểm năm3;pass_calculus2|Xác nhậnGT2đãpass;read_data_structures_fail|XemCTDLchưađạt;read_78_credits|Xem78tín chỉ;receive_lab_request|Đạm nhờ mang sách'),
('Lab','take_book|Lấy sách;travel_hcmus|Đến labHCMUS;deliver_book|Giao sách;listen_lab_talk|Nghe thảo luận;return_room|Về404'),
('Room','edit_cv|ChỉnhCV;export_cv_pdf|XuấtPDF;send_cv|GửiCV;receive_job_email|Vài ngày sau nhận mail đậu'),
('Room','see_to_to_work|Tiễn Tộ đi làm;read_friend_schedules|Xem lịch bạn bè;receive_eligibility_notice|Nhận thông báo xét đồ án'),
('Room','apply_thesis|Nộp đơn;read_rejection|Xem118/136và2tiênquyết;receive_photo_invite|Nhận lời chụp ảnh'),
('Courtyard','meet_graduate_friends|Gặp ba bạn sau lễ riêng;take_group_photo|Chụp ảnh chung;answer_parents|Trả lời phụ huynh;return_room|Về404'),
('Room','carry_longs_boxes|Bê thùng cho Long;say_goodbye_long|Chào Long;long_leaves|Long rời phòng;freshman1_moves_in|Người mới1nhận giường'),
('Room','receive_to_gift|Nhận thịt khô;say_goodbye_to|Chào Tộ;to_leaves|Tộ chuyển trọ;freshman2_moves_in|Người mới2nhận giường'),
('Room','hear_dams_plan|Nghe lịch đi của Đạm;say_goodbye_dam|Chào Đạm;dam_leaves_dorm|Đạm rờiKTX;freshman3_moves_in|Người mới3nhận giường'),
('Room','select_uninstall|Chọn xóa game;hold_uninstall|Giữ đúng nút đến hoàn tất;set_alarm|Đặt06:30;write_recovery_plan|Lập lịch trả nợ'),
('H6','wake_immediately|Dậy06:30;morning_wc|Vệ sinh;arrive_class_on_time|Tới lớp đúng giờ;study_days|Chuỗi ngày học đại diện;hear_knock|Nghe gõ cửa khuya'),
('Room','open_for_friends|Mở cửa;receive_supplies|Nhận tiếp tế;share_hotpot|Ăn lẩu;receive_study_notes|Nhận tài liệu;say_goodbye_visitors|Tiễn bạn;read_flight_message|Đạm báo sắp bay'),
('H6','revise_data_structures|ÔnCTDL;pass_data_structures|PassCTDLkỳ9;enroll_operating_systems|HọcHĐHkỳ10;pass_operating_systems|PassHĐH;read_debt_cleared|Xem hết nợ'),
('Room','open_eligibility_portal|Mở portal;confirm_136_credits|Xác nhận136/150;receive_advisor_appointment|Nhận lịchGVHD'),
('AdvisorOffice','meet_advisor|Gặp thầy;choose_network_topic|Chọn đề tài mạng;register_topic|Đăng ký đề tài;receive_research_checklist|Nhận checklist'),
('Library','search_references|Tra cứu;assemble_network_design|Ghép sơ đồ;validate_design|Xác nhận thiết kế;write_backlog|Lập backlog'),
('Room','implement_modules|Viết module;run_project_tests|Chạy test;run_first_demo|Demo đầu chạy được;book_demo1|Đặt lịchdemo1'),
('AdvisorOffice','present_demo1|Chạy demo1;record_security_feedback|Ghi việc sửa;patch_security|Sửa bảo mật;pass_security_tests|Test mới pass;book_demo2|Hẹn demo2'),
('AdvisorOffice','pass_demo2|Demo2được duyệt;format_report|Định dạng120trang;print_report|In;bind_thesis|Đóng bìa;review_submission_deadline|Xem hạn nộp'),
('Room','reproduce_crash|Tái hiệncrash;read_crash_log|Đọc log;patch_final_bug|Patch;pass_final_tests|Test lại;submit_project|Nộp trước hạn;save_receipt_backup|Lưu receipt vàbackup;receive_defense_schedule|Nhận lịch bảo vệ'),
('DefenseRoom','change_to_shirt|Thay sơ mi;connect_projector|Cắm máy chiếu;present_thesis|Thuyết trình;pass_defense_qa|Trả lời phản biện đạt;receive_grade_a|Nhận kết quảA'),
('Room','call_mother|Gọi mẹ;pack_own_belongings|Gói đồ riêng;turn_off_light|Tắt đèn;close_room_door|Khép cửa;return_key|Trả khóa;receive_ceremony_invitation|Nhận thư mời'),
('GraduationHall','receive_degree|Nhận bằngA5;meet_family|Gặp gia đình;read_dam_return_message|Đạm nhắn đã về;reunite_friends|Hội ngộ bạn;travel_binh_dong|Tới BìnhĐông;link_shoulders|Khoác vai;finish_credits|Xem hếtcredits')]
assert len(rows)==45
missions=[]
for i,(zone,row) in enumerate(rows,1):
 steps=[]
 for pair in row.split(';'):
  id,label=pair.split('|');steps.append(dict(id=id,label=label,zone=zone,cost=0,reward=0,credits=0,requires=[],grants=[],removes=[],choices=[]))
 base=plan['missions'][i-1]
 missions.append(dict(id=i,entry=base['entry'],bridge=base['exit'],steps=steps))
curriculum=json.loads((ROOT/'mobile-2d/Assets/Resources/ProductionCampaign/year-one-courses.json').read_text())['courses']
assert len(curriculum)==6 and sum(c['credits'] for c in curriculum)==32
# Stable individual lesson/exam events allow reload between courses without granting a year in one click.
year=missions[6]['steps'];index=next(i for i,s in enumerate(year) if s['id']=='finish_year_exams')
extra=[dict(id='review_commute_ledger',label='Dự trù vé các buổi học còn lại',zone='H6Classroom',cost=63000,reward=0,credits=0,requires=[],grants=['ledger:year1_commute_reserved'],removes=[],choices=[])]
cumulative=0
for course in curriculum:
 cumulative+=course['credits']
 for kind in ['study','exam']:
  extra.append(dict(id=f'{kind}_y1_'+course['id'],label=('Học: ' if kind=='study' else 'Thi: ')+course['name'],zone='H6Classroom',cost=0,reward=0,credits=cumulative if kind=='exam' else 0,requires=['ledger:year1_commute_reserved']+([f'action:7:study_y1_'+course['id']] if kind=='exam' else []),grants=['passed:Y1_'+course['id']] if kind=='exam' else [],removes=[],choices=[]))
year[index:index]=extra
def step(m,id):return next(s for s in missions[m-1]['steps'] if s['id']==id)
def effect(m,id,**kw):step(m,id).update(kw)
def option(id,label,grants):return dict(id=id,label=label,cost=0,reward=0,requires=[],grants=grants,removes=[])
effect(1,'buy_ticket',cost=7000,grants=['item:dorm_ticket'])
effect(1,'arrive_dorm',requires=['item:dorm_ticket'],removes=['item:dorm_ticket'])
effect(2,'present_documents',requires=['item:admission_documents'],grants=['room:assigned'],removes=['item:admission_documents'])
effect(2,'receive_room_key',requires=['room:assigned'],grants=['item:room_key'])
effect(3,'claim_bed',choices=[option('left','Giường bên trái',['bed:luon_left']),option('right','Giường bên phải',['bed:luon_right'])])
effect(3,'unlock_room',requires=['item:room_key']);effect(3,'place_backpack',requires=['item:backpack'],grants=['room:backpack_stored'])
effect(3,'repair_fan',requires=['item:screwdriver'])
effect(4,'buy_cleaning_kit',cost=5000,grants=['item:cleaning_kit']);effect(4,'clean_four_spots',requires=['item:cleaning_kit'])
meal_options=[option('vegetarian','Cơm và rau',['meal:vegetarian']),option('meat','Cơm và thịt',['meal:meat']),option('standard','Cơm, thịt và rau',['meal:standard']),option('vegetarian_tea','Cơm, rau và trà',['meal:vegetarian_tea']),option('meat_tea','Cơm, thịt và trà',['meal:meat_tea']),option('tea','Cơm, thịt, rau và trà',['meal:tea'])]
for item,cost in zip(meal_options,[10000,15000,20000,13000,18000,23000]):item['cost']=cost
effect(5,'choose_meal',choices=meal_options);effect(5,'receive_schedule',reward=100000,grants=['schedule:year1'])
effect(6,'equip_backpack',requires=['item:backpack','room:backpack_stored']);effect(6,'take_bus',cost=7000);effect(6,'return_room',cost=7000)
effect(7,'catch_bus',choices=[option('on_time','Chuyến đúng giờ',['travel:on_time']),option('next_bus','Chuyến sau',['travel:missed_bus_recovered'])],cost=7000)
effect(7,'finish_year_exams',requires=['passed:Y1_'+c['id'] for c in curriculum],grants=['ledger:year1_commute_used'])
effect(7,'read_year_results',zone='Room',credits=32,requires=['ledger:year1_commute_used']+['passed:Y1_'+c['id'] for c in curriculum])
effect(7,'join_wc_queue',zone='Hallway');effect(7,'attend_class',zone='H6Classroom');effect(7,'finish_year_exams',zone='H6Classroom')
effect(8,'get_cooker',requires=['credits:32']);effect(8,'plug_cooker',requires=['action:8:get_cooker']);effect(8,'add_ingredients',requires=['action:8:plug_cooker']);effect(8,'share_feast',requires=['action:8:add_ingredients']);effect(8,'toast',requires=['action:8:share_feast'])
effect(8,'sleep_new_term',requires=['credits:32','action:8:toast'],grants=['term:3'])
effect(9,'confirm_schedule',grants=['schedule:0645'])
effect(10,'choose_night',choices=[option('play','YES — chơi tiếp',['choice:gaming_yes']),option('sleep','NO — đi ngủ',['choice:gaming_no'])])
effect(11,'choose_alarm',choices=[option('wake','Dậy ngay',['choice:wake']),option('snooze','Snooze',['choice:snooze'])])
effect(12,'resolve_inspection',grants=['story:inspection_resolved'])
effect(13,'submit_assignment',requires=['action:13:test_assignment'],grants=['submission:cpp'])
effect(15,'read_failed_courses',credits=50,grants=['failed:GT2','failed:VL1','failed:KTLT'])
effect(16,'answer_mother',choices=[option('honest','Nói thật',['choice:mother_honest']),option('ask_help','Xin hỗ trợ',['choice:mother_help'])])
effect(17,'backup_data',grants=['job1:backup']);effect(17,'install_system',requires=['job1:backup']);effect(17,'handover_laptop',reward=50000)
effect(19,'complete_job_a',reward=50000);effect(19,'complete_job_b',reward=50000)
effect(19,'review_weekly_ledger',reward=1200000,grants=['ledger:weeks_reviewed']);effect(19,'pay_retaking_fee',cost=900000,requires=['ledger:weeks_reviewed'])
effect(19,'pass_programming',grants=['passed:KTLT'],removes=['failed:KTLT']);effect(19,'pass_physics',grants=['passed:VL1'],removes=['failed:VL1']);effect(19,'enroll_data_structures',requires=['passed:KTLT'],grants=['enrolled:CTDL'])
effect(20,'waive_labor',grants=['job:waived_labor_only'])
effect(21,'borrow_jacket',grants=['item:borrowed_jacket']);effect(21,'return_jacket',requires=['item:borrowed_jacket'],removes=['item:borrowed_jacket'])
effect(22,'share_tea',grants=['friends:shared_tea'])
effect(23,'pass_calculus2',grants=['passed:GT2'],removes=['failed:GT2']);effect(23,'read_data_structures_fail',requires=['enrolled:CTDL'],grants=['failed:CTDL']);effect(23,'read_78_credits',credits=78)
effect(27,'read_rejection',credits=118,requires=['failed:CTDL'],grants=['thesis:rejected'])
for m,who,n in [(29,'long',1),(30,'to',2),(31,'dam',3)]:
 id={'long':'long_leaves','to':'to_leaves','dam':'dam_leaves_dorm'}[who]
 effect(m,id,grants=['room:'+who+'_left'])
 effect(m,f'freshman{n}_moves_in',requires=['room:'+who+'_left'],grants=[f'room:freshman{n}'])
effect(32,'hold_uninstall',grants=['game:uninstalled'])
effect(33,'wake_immediately',requires=['game:uninstalled'])
effect(34,'open_for_friends',requires=['room:dam_left']);effect(34,'read_flight_message',grants=['dam:flight_after_visit'])
effect(35,'pass_data_structures',requires=['passed:KTLT'],grants=['passed:CTDL'],removes=['failed:CTDL'])
effect(35,'enroll_operating_systems',requires=['passed:CTDL'],grants=['enrolled:HDH'])
effect(35,'pass_operating_systems',requires=['passed:CTDL','enrolled:HDH'],grants=['passed:HDH'])
effect(36,'confirm_136_credits',credits=136,requires=['passed:GT2','passed:VL1','passed:KTLT','passed:CTDL','passed:HDH'],grants=['thesis:eligible'])
effect(37,'register_topic',requires=['thesis:eligible'],grants=['thesis:network_diagnostics'])
effect(38,'validate_design',requires=['thesis:network_diagnostics'],grants=['thesis:design_valid'])
effect(39,'run_first_demo',requires=['thesis:design_valid'],grants=['thesis:demo1_ready'])
effect(40,'pass_security_tests',requires=['thesis:demo1_ready'],grants=['thesis:security_tests_pass'])
effect(41,'pass_demo2',requires=['thesis:security_tests_pass'],grants=['thesis:demo2_pass']);effect(41,'bind_thesis',requires=['thesis:demo2_pass'],grants=['item:bound_thesis'])
effect(42,'submit_project',requires=['action:42:pass_final_tests','item:bound_thesis'],grants=['thesis:submitted']);effect(42,'save_receipt_backup',requires=['thesis:submitted'],grants=['thesis:receipt'])
effect(43,'connect_projector',requires=['thesis:receipt']);effect(43,'pass_defense_qa',requires=['thesis:receipt'],grants=['thesis:defense_pass']);effect(43,'receive_grade_a',requires=['thesis:defense_pass'],credits=150,grants=['thesis:grade_a'])
effect(44,'pack_own_belongings',requires=['thesis:grade_a','room:freshman1','room:freshman2','room:freshman3'],grants=['room:luon_packed'])
effect(44,'return_key',requires=['item:room_key','room:luon_packed'],removes=['item:room_key'],grants=['room:key_returned'])
effect(45,'receive_degree',requires=['room:key_returned','thesis:grade_a'],grants=['item:degree']);effect(45,'finish_credits',requires=['item:degree','action:45:link_shoulders'],grants=['campaign:finished'])
for m,id,zone in [(1,'arrive_dorm','Reception'),(2,'reach_room','Room'),(5,'return_sleep','Room'),(6,'equip_backpack','Room'),(6,'wash_before_class','Hallway'),(6,'enter_class','H6Classroom'),(6,'solve_warmup','H6Classroom'),(6,'finish_lesson','H6Classroom'),(6,'return_room','Room'),(6,'tell_dam','Room'),(21,'borrow_jacket','Room'),(21,'return_jacket','Room'),(21,'accept_lake_invite','Room'),(22,'return_sleep','Room'),(24,'take_book','Room'),(24,'return_room','Room'),(28,'return_room','Room'),(33,'hear_knock','Room'),(40,'patch_security','Room'),(40,'pass_security_tests','Room'),(41,'format_report','Room'),(41,'print_report','PrintShop'),(41,'bind_thesis','PrintShop'),(44,'return_key','Reception'),(44,'receive_ceremony_invitation','Reception'),(45,'travel_binh_dong','BinhDong'),(45,'link_shoulders','BinhDong'),(45,'finish_credits','BinhDong')]:
    step(m,id)['zone']=zone
compatibility=json.loads((ROOT/'story/plans/PRODUCTION_SAVE_COMPATIBILITY.json').read_text())
result=dict(version=1,compatibleSaves=compatibility,scope='Production event contracts for45missions; scene/minigame adapters still required',initialMoney=35000,initialFacts=['item:admission_documents','item:backpack','item:screwdriver'],missions=missions)
target=ROOT/'mobile-2d/Assets/Resources/ProductionCampaign/missions.json';target.write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n')
print('Authored',len(missions),'missions,',sum(len(m['steps']) for m in missions),'ordered gameplay events')
