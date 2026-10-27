# Plan asset hoàn chỉnh trước khi generate — Phòng 404

Ngày cập nhật: 25/10/2026. **Đang thực thi: 24/24 sheet đã generate/slice, 178/178 output.**

Tiến độ thực tế: `assets/mvp_sprites/remaining/progress.json` và `GENERATION.md`. Không dùng trạng thái dự kiến phía dưới làm bằng chứng đã hoàn thành.
Phạm vi: **toàn bộ 45 mission**, từ bến xe đến hội ngộ; thêm UI, dữ liệu, audio và hoàn thiện M6.
Nguồn: MASTER_GAME_FLOW.md; FULL_FLOW_AUDIT_BEFORE_GENERATION.md; source Unity và asset hiện có.

Đây là baseline sản xuất áp dụng các đề xuất của báo cáo flow vào danh mục asset. Model tiến độ đã triển khai; prototype production M01–M09 đã nối gameplay. M10–M45 chưa được xác nhận chơi xuyên suốt. Kịch bản gốc được giữ làm nguồn đối chiếu. Không dùng master cũ để tự thêm lại các asset đã loại trong plan này.

## Chốt phạm vi trước lượt generate tiếp theo — 25/10/2026

**Danh mục đã liệt kê đủ cho baseline; chưa có batch ảnh bổ sung được lên lịch.** Dùng checklist từng ô ở tài liệu liên kết bên dưới để review, không dùng bản dán M2–M5 làm danh mục sản xuất.

| Phần việc | Hiện trạng / bước tiếp theo |
|---|---|
| 46 pose/NPC + 46 props + 12 nền + 74 frame | Đã có 24 sheet và 178 output; ưu tiên dùng lại, không tạo lại toàn bộ |
| 27 nhóm ảnh cũ | Tái sử dụng; derived crop/normalize phải giữ nguồn và không cộng vào 178 ảnh mới |
| 29 nhóm UI/hệ thống | Dựng bằng Unity; chữ, đồng hồ, giá, bảng điểm, BIOS, slide và tiến trình là nội dung tương tác |
| M01–M09 | Prototype gameplay819checks qua hai giường/hai kết quả xe; props WC/lẩu đã bind; contact/motion/audio/device/fullgame còn pending |
| M07–M08 | Đã nối queue/deadline/chuyến sau/6môn/ledger/lẩu→M09; còn props WC/lẩu và motion chi tiết. Xem PRODUCTION_YEAR_ONE_REVIEW.md |
| M09–M45 | Còn triển khai/tích hợp interaction và transition theo baseline; model dữ liệu không đồng nghĩa đã chơi được |
| Animation/staging | Còn duyệt chuyển động, AN01 backpack-back bob, AN05 silhouette, ghép tay/đạo cụ và foreground trong scene |
| Data/audio/M6 | Danh sách tại mục 9; còn thoại/config/cue, touch/safe area, save/load, build và full playthrough |

Trước mỗi batch bổ sung/refine phải ghi: lỗi quan sát + mission, kết quả thử reuse, ID/frame chính xác, grid/cell/lề/thứ tự ô, reference/prompt/source, cách slice/pivot/PPU/anchor và tiêu chí nghiệm thu. Chỉ phát sinh ảnh theo điều kiện ở mục 10; không dùng ảnh để chữa lỗi flow/input/save.

## Cập nhật theo yêu cầu liệt kê trước generate

Danh sách từng asset/frame theo thứ tự ô nằm tại [ASSET_PRODUCTION_CHECKLIST.md](ASSET_PRODUCTION_CHECKLIST.md); JSON là nguồn ID/crop/mission và trạng thái máy đọc. Không gọi imagegen trong lần cập nhật plan này.

Danh sách M2–M5 đã dán **chưa phải danh sách đầy đủ cho toàn bộ phần còn lại**: thiếu mở đầu M01–05, cầu nối và animation; đồng thời có UI/text và asset đã tồn tại cần tái sử dụng. Không áp dụng tuyên bố “mapping trực tiếp với scripts” trong bản dán khi chưa kiểm chứng reference runtime.

24 sheet/178 output hiện đã có nguồn trong workspace. Bước tiếp theo là QA chuyển động, staging/import và tích hợp theo các gate; không generate lại toàn danh sách. Mọi phát sinh hoặc refine phải cập nhật plan và layout trước lượt gọi tiếp theo.

## 1. Quyết định baseline và phạm vi

- Cốt truyện tuyến tính, lựa chọn nhỏ có hậu quả thật rồi hội tụ; không thiết kế thêm ending học tốt/tránh toàn bộ arc.
- M01–05 được tính đầy đủ. M06 có bạn cùng lớp Bách Khoa; Đạm phản ứng tại 404. M07 hụt xe dùng điểm đón phụ/chuyến sau.
- M08→M09 đăng ký→M10, không nhảy thẳng gaming. M15→M16→M17; M32→M33–36→M37; M43→M44→M45→credits.
- Giải Tích 2 đã pass ở năm 3 thì không thi lại năm 5. Graph game: KTLT→CTDL→HĐH. Credits mẫu cuối năm:32/50/78/118/136/150, là cân bằng hư cấu cho game, không phải quy chế trường thật.
- M27 thiếu 18 so với ngưỡng 136 để làm đồ án. M36 đạt 136/150; năm 6 hoàn tất 14 còn lại qua học phần đồ án cấu hình trong game.
- Đề tài thống nhất: **Hệ thống tự động hóa và chẩn đoán lỗi thiết bị mạng**.
- Đạm rời KTX nhưng chưa bay trước M34; tin nhắn giải thích lần trở về M45. Ba bạn rời đi được thay bằng ba người mới.
- M44 chỉ góc Lươn dọn trống, đồ người mới giữ nguyên; có bước trả khóa. M45 mới nhận bằng vật lý.
- Hội đồng 5 người: GVHD dùng visual teacher có sẵn +4 giám khảo mới. Không thêm một advisor khác rồi vô tình thành 6 người.
- Một bối cảnh Bình Đông rộng, camera crop; không tạo góc phụ chỉ để lấp batch.
- Trang phục: thường phục trong 404/code đêm; sơ mi khi gặp GVHD/bảo vệ; áo tốt nghiệp tại lễ; đổi thường phục trước bữa hội ngộ.

Các câu thoại, số liệu và thời điểm đều là data/UI. M13 hạn 23:59:59, submit mẫu 23:59:41. M42 baseline đề xuất 22:45→23:45, submit mẫu 23:44:32; không bake giờ vào ảnh.

## 2. Tổng khối lượng và cách đọc

- **R**: file có sẵn để tái sử dụng. **N**: ảnh mới trong batch. **AN**: frame animation mới (đã tính trong N). **U**: dựng bằng Unity. **D/A**: dữ liệu và âm thanh, không imagegen.
- Một ô là một output dự kiến, không phải một prefab/mission. Crop con từ cùng ô hoặc foreground từ background là derived asset, không tính thành lượt generate mới.
- Đây là plan đủ để chuẩn bị từng batch. Trước khi gọi imagegen cho một batch phải xác nhận blockout/camera/anchor đúng; chưa kiểm chứng gameplay đầy đủ chỉ bằng tài liệu này.
- Trạng thái từng batch/asset có trong JSON. Batch đã thực thi có source và actual crop; các batch chưa tạo vẫn null. Chưa xác nhận hoàn thành toàn bộ gameplay.

| Hạng mục | Số lượng |
|---|---:|
| Mission được map | 45 |
| Nhóm tái sử dụng | 27 |
| Nhóm Unity | 29 |
| Batch dự kiến | 24 |
| Pose/NPC mới | 46 |
| Props mới | 46 |
| Bối cảnh mới | 12 |
| Frame animation mới | 74 |
| Tổng ô/output ảnh mới | **178** |

Số trên thay thế dự toán 16 sheet/98 ô trước đây. Nếu blockout cho thấy dùng được asset cũ, giảm/remove ô và cập nhật JSON cùng tài liệu; không generate lại chỉ để giữ con số. Không tính UI/text/VFX/audio vào số ảnh.

## 3. Asset cũ dùng lại

Đã rà file; kiểm tra trực quan mẫu Lươn/walk/backpack, bạn bè, study, room, H6, lẩu, teacher, chổi và bằng. Chưa tuyên bố mọi asset đã đạt QA trong mọi scene. Walk_0–3 là chu kỳ cùng hướng; cần front/back theo mục 7.

| ID | File đại diện từ mobile-2 d/Assets/Sprites/MVP/ | Dùng tiếp |
|---|---|---|
| R_LUON | `characters/luon_idle_0.png` | Lươn thường phục idle; walk_0–3 cùng hướng, flip trái/phải |
| R_WALK | `characters/luon_walk_0.png` | Chu kỳ ngang hiện có gồm walk_0–3 |
| R_BACKPACK | `dorm_journey/backpack_idle.png` | Idle + backpack_walk_0–2; một hướng có flip |
| R_BED | `characters/bed_interaction/luon_bed_sleeping.png` | Bộ enter/idle/sleep/exit hiện có |
| R_STUDY | `commute/study_0.png` | Study_0–2 và notebook; desk lower/foreground native crop |
| R_TYPE | `commute/typing_0.png` | Typing_0–1 và sprite ngồi/laptop; không dùng cho sơ mi nếu lộ sai áo |
| R_FRIENDS | `characters/dam_standing.png` | dam/long/to standing và sitting; nhận diện canonical |
| R_ROOM | `environment/room_404_backdrop.png` | Phòng nền trống + furniture lớp rời từ scene |
| R_DESK | `environment/study_desk_chairs.png` | Bàn/ghế và foreground native crop |
| R_BUNK | `environment/bunk_bed_lengthwise.png` | Giường tầng có sẵn, không đổi perspective bedding |
| R_SWITCH | `environment/light_switch_socket.png` | Công tắc/ổ điện, animation ánh sáng bằng Unity |
| R_HALL | `dorm_journey/hallway.png` | Hành lang; camera/foreground có sẵn |
| R_WC | `dorm_journey/wc_male.png` | WC Nam/Nữ; cửa/water/progress đã có prototype |
| R_CAMPUS | `commute/campus.png` | Đường đi bộ/trạm; chỉnh camera thay vì tạo lại |
| R_BUS | `commute/bus_33.png` | Xe buýt 33 và bus_sign; hai chuyến khác ngày dùng cùng asset |
| R_H6 | `year1/h6_classroom_v2.png` | H6 gate_v 2 + classroom_v 2, không dùng revision cũ |
| R_TEACHER | `year1/teacher.png` | Giảng viên generic; tái dùng visual cho GVHD/giáo sư lab với identity data riêng |
| R_POT | `year1/pot_0.png` | Pot_0/pot_1, steam; cùng một lẩu, không tạo lại |
| R_FLOOR | `year1/luon_floor.png` | luon/dam/long/to_floor: pose ăn sàn, M08/M34 |
| R_LAPTOP | `props/laptop_open.png` | Laptop nguyên vẹn; screen là overlay |
| R_BAGPROP | `props/luon_backpack.png` | Balo prop rách, thu dọn M44 |
| R_TRASH | `props/trash_piles.png` | trash_noodles/cans/bags/small và piles; bật/tắt theo năm |
| R_CLEAN | `props/cleaning_broom_dustpan.png` | Chổi/hót rác; bucket/rag là mới |
| R_DIPLOMA | `props/diploma_degree.png` | Bằng hiện có, không generate lại; sửa nhãn bằng runtime nếu cần |
| R_DIALOGUE | `ui/dialogue_box.png` | Dialogue box, portrait_luon/dam/long/to, button_yes/no |
| R_GRADES | `ui/grade_stamp_f.png` | Stamp F và A đã có; không generate lại |
| R_SHADOW | `shadows/shadow_character.png` | Shadow character/bed/desk/small tách khỏi sprite |

Không dùng dam_writing như Đạm trong lớp Bách Khoa; không generate lại room/H6/A/F/bằng/rác/lẩu. Nguồn sheet cũ giữ đúng provenance, không đổi tên thành batch mới. Bản ghép kiểm tra mẫu: [existing_asset_contact_sheet.jpg](plans/existing_asset_contact_sheet.jpg), chỉ ghép ảnh cũ, không generate mới. Danh sách toàn bộ PNG/dimensions: [EXISTING_SPRITE_INVENTORY.json](plans/EXISTING_SPRITE_INVENTORY.json).

## 4. Mapping toàn bộ 45 mission

Các ID N dẫn tới mục 5–7; R ở mục 3; U ở mục 8. Animation batch kèm theo đã tính trong tổng frame, không cộng thêm lần nữa. Dữ liệu entry/action/exit chi tiết nằm trong JSON và báo cáo flow.

### M1 — Năm 1

| Mission | R | N cụ thể | AN batch | U | Điều cần giữ trong flow |
|---|---|---|---|---|---|
| M01 | R_LUON, R_WALK, R_BACKPACK, R_BUS | npc_station_tout, nem_chua_box, bus_station_background | AN00, AN01 | U_HUD, U_TRAVEL, U_INVENTORY | Ví chung; vé đầu game khác chuyến đi học; không phạt lặp đến kẹt |
| M02 | R_DIALOGUE, R_HALL | npc_dorm_manager, keys_generic, dorm_reception_background | Không | U_INVENTORY, U_DIALOGUE | Nhận đúng giấy/chìa khóa rồi mới được vào404 |
| M03 | R_ROOM, R_BUNK, R_SWITCH, R_BAGPROP, R_LUON | nem_chua_box, mini_fan, screwdriver_kit | AN00, AN01 | U_INVENTORY, U_ROOM_STATE | Xí chỗ/đặt balo trước; lấy balo đi học là bước sau |
| M04 | R_ROOM, R_CLEAN, R_TRASH, R_FRIENDS | npc_canteen_vendor, cleaning_bucket, cleaning_rag, dried_buffalo_meat, canteen_background | AN00 | U_CLEAN, U_INVENTORY, U_DIALOGUE | Mua một lần, timeout chỉ thử lại phần dọn |
| M05 | R_FRIENDS, R_LUON, R_TYPE | npc_canteen_vendor, meal_tray, food_rice, food_meat, food_vegetables, canteen_background, luon_lake_sit, dam_lake_sit, long_lake_sit, to_lake_sit | AN00 | U_MEAL, U_DIALOGUE | Pose ngồi cũ là ngồi sàn; dùng chung bốn pose ghế C02 với M22, không thêm sheet. |
| M06 | R_H6, R_TEACHER, R_STUDY, R_BUS, R_BACKPACK, R_HALL, R_WC | npc_h6_classmate_writing, toothbrush_cup, towel_folded | AN00, AN01 | U_CLASS, U_TRAVEL, U_DIALOGUE | Bạn cùng lớp thay Đạm; phản ứng Đạm về404; chưa kết thúc năm |
| M07 | R_WC, R_HALL, R_CAMPUS, R_BUS, R_BACKPACK, R_H6, R_TEACHER, R_STUDY | npc_freshman_1, npc_freshman_2, npc_freshman_3, toothbrush_cup, towel_folded, npc_h6_classmate_writing | AN00, AN01 | U_TRAVEL, U_WAKE | Queue dùng3freshman làm NPC generic không gán tên; hụt xe có điểm đón phụ |
| M08 | R_ROOM, R_POT, R_FLOOR, R_SWITCH | dried_buffalo_meat, drinking_glass, food_vegetables | Không | U_DIALOGUE, U_ACADEMIC, U_VFX | Có tương tác lấy/cắm/nấu;32/150; sang kỳ3 mở M09 |

### M2 — Năm 2

| Mission | R | N cụ thể | AN batch | U | Điều cần giữ trong flow |
|---|---|---|---|---|---|
| M09 | R_ROOM, R_LAPTOP, R_TYPE | Không | AN00 | U_DESKTOP, U_ACADEMIC | 08:59 đăng ký; biết lớp06:45 trước lựa chọn đêm |
| M10 | R_ROOM, R_LAPTOP, R_TYPE, R_TRASH | luon_tired_idle, long_gaming_shout | AN00 | U_GAMING, U_TIMELAPSE | NO đi ngủ thật; hội tụ với flags khác; YES tới04:32 |
| M11 | R_BED, R_ROOM | luon_tired_idle, alarm_clock_blank | AN00 | U_WAKE, U_ACADEMIC | 06:30→11:47 nếu snooze; cúp tiết, không bỏ thi |
| M12 | R_ROOM, R_POT, R_BED, R_SWITCH | luon_carry_hotpot, guard_idle, guard_inspecting, wardrobe_closed, wardrobe_open_empty, keys_generic, flashlight | AN00, AN04 | U_STEALTH, U_VFX | Tủ mở/đóng + nồi overlay; guard đi ngang; retry phục hồi state |
| M13 | R_LAPTOP, R_TYPE | Không | Không | U_CODE, U_DESKTOP | Hạn23:59:59; submit mẫu23:59:41, không bake chữ |
| M14 | R_H6, R_DESK, R_STUDY | luon_exam_worried, luon_exam_bite_pen | AN00 | U_EXAM | Thi có tiến độ theo phần kiến thức, không ép người chơi thất bại input |
| M15 | R_GRADES, R_LAPTOP | Không | Không | U_ACADEMIC, U_PHONE | GT2/VL1/KTLT F; graph khóa môn rõ, không khóa mọi thứ |
| M16 | R_HALL, R_DIALOGUE | luon_phone_sad | AN00 | U_PHONE, U_DIALOGUE, U_ACADEMIC | Hai cách trả lời mẹ đều có lối tới job đầu; tổng nợ và phí kỳ riêng |

### M3 — Năm 3

| Mission | R | N cụ thể | AN batch | U | Điều cần giữ trong flow |
|---|---|---|---|---|---|
| M17 | R_DESK, R_LAPTOP, R_TYPE | npc_repair_customer, luon_repair_laptop, usb_boot, screwdriver_kit, savings_jar, cash_notes_blank | Không | U_INSTALL, U_JOBS, U_INVENTORY | Backup giả lập trước format;50k chỉ nhận một lần |
| M18 | R_ROOM, R_LAPTOP | npc_repair_customer, usb_boot, laptop_open_case, service_sign_blank | Không | U_JOBS, U_PHONE | Biển trống chữ; job làm trên thiết bị/nhà mạng được đồng ý |
| M19 | R_H6, R_STUDY, R_TYPE, R_BACKPACK | npc_repair_customer, luon_tired_idle, luon_repair_laptop, usb_boot, screwdriver_kit, laptop_open_case, savings_jar, cash_notes_blank, service_sign_blank, bread, technician_bag | AN00, AN01 | U_JOBS, U_TIMELAPSE, U_ACADEMIC | 3job đại diện + nhiều tuần thu/chi; nghỉ miễn phí nếu energy0 |
| M20 | R_DESK, R_LAPTOP, R_FRIENDS | crush_idle, luon_repair_laptop, screwdriver_kit | Không | U_REPAIR, U_JOBS, U_DIALOGUE | Miễn phí chỉ khoản công50k, không xóa nợ toàn game |
| M21 | R_LAPTOP, R_TYPE | crush_idle, crush_boyfriend_idle, luon_repair_laptop, luon_borrowed_jacket, screwdriver_kit, laptop_open_case, scooter_white, tea_food_cart_blank, street_lamp, peach_tea, drink_cafe_background | AN00 | U_PHONE, U_REPAIR, U_DIALOGUE | Mượn/trả áo; đổi objective có tin hẹn và điểm đến |
| M22 | R_DIALOGUE | luon_lake_sit, dam_lake_sit, long_lake_sit, to_lake_sit, lake_bench, tea_food_cart_blank, street_lamp, peach_tea, rice_paper_snack, drinking_glass, lake_background | AN00 | U_DIALOGUE, U_VFX | 4pose ngồi ghế mới; Long mời nếu thiếu tiền; không timer |
| M23 | R_LAPTOP | Không | Không | U_ACADEMIC, U_TIMELAPSE | 78/150; pass GT2; có bước kỳ5 KTLTpass→kỳ6CTDL trước khi CTDLfail |

### M4 — Năm 4–5

| Mission | R | N cụ thể | AN batch | U | Điều cần giữ trong flow |
|---|---|---|---|---|---|
| M24 | R_TEACHER, R_STUDY, R_BACKPACK | dam_lab_pose, research_lab_background | AN00, AN01 | U_TRAVEL, U_INVENTORY, U_DIALOGUE | Lab HCMUS; visual professor generic reuse, identity riêng |
| M25 | R_LAPTOP, R_TYPE, R_FRIENDS | Không | Không | U_CV_WORD, U_PHONE | CV xuất PDF, vài ngày sau mail đậu; cùng framework Word |
| M26 | R_ROOM, R_FRIENDS | long_office, to_office | Không | U_ROOM_STATE, U_DIALOGUE | Ban ngày vắng do lịch đi làm, chưa xóa bạn khỏi save |
| M27 | R_LAPTOP | Không | Không | U_ACADEMIC | Mốc kế hoạch118/136; thiếu18 để làm đồ án, không so với150 |
| M28 | R_LUON, R_DIALOGUE | dam_graduate, long_graduate, to_graduate, father_luon, mother_luon, camera, vnu_photo_courtyard_background | AN00 | U_PHOTO, U_TRAVEL | Chụp chung sau các lễ riêng; mẹ/bố sprite M45 làm phụ huynh qua palette/crop |
| M29 | R_ROOM, R_HALL, R_BUNK | npc_freshman_1, long_office, luon_carry_box, moving_box_blank, suitcase, bedding_freshman_a | AN00, AN04, AN05 | U_ROOM_STATE, U_INVENTORY, U_TRAVEL | Long đi→giường trống→freshman1; không spawn cả ba ngay |
| M30 | R_ROOM, R_HALL, R_BUNK | npc_freshman_2, to_office, suitcase, bedding_freshman_b, dried_buffalo_meat | AN00, AN05 | U_ROOM_STATE, U_INVENTORY, U_DIALOGUE | Tộ tặng thịt, chào/ôm bằng staging; freshman2 vào |
| M31 | R_ROOM, R_HALL, R_BUNK | npc_freshman_3, dam_travel, suitcase, bedding_freshman_c | AN00, AN05 | U_ROOM_STATE, U_DIALOGUE | Đạm đi sau cùng;freshman3; chưa bay trước lần tiếp tế |
| M32 | R_LAPTOP, R_TYPE | Không | Không | U_UNINSTALL | Giữ đúng nút; chưa được nhận đồ án hay A |
| M33 | R_BED, R_WC, R_H6, R_BACKPACK | npc_freshman_1, npc_freshman_2, npc_freshman_3, alarm_clock_blank, toothbrush_cup, towel_folded, bedding_freshman_a, bedding_freshman_b, bedding_freshman_c | AN00, AN01 | U_WAKE, U_TRAVEL, U_ACADEMIC | 06:30 dậy ngay; học/trả nợ, không đổi thành06:00 |
| M34 | R_ROOM, R_POT, R_FLOOR, R_STUDY | long_office, to_office, dam_travel, hotpot_supply_bag, beer_supply, drinking_glass | Không | U_DIALOGUE, U_VFX, U_PHONE | Trước khi Đạm bay; pose sàn gắn khuôn mặt/áo công sở bằng rig/crop nếu cần |
| M35 | R_H6, R_STUDY | Không | AN00 | U_EXAM, U_ACADEMIC | CTDL kỳ9/10 → PASS mới mở HĐH kỳ11 Năm6; không thi lại GT2 đãpass |
| M36 | R_LAPTOP | Không | Không | U_ACADEMIC | Đầu Năm6 sau điểm môn cuối: 136/150, đủ tiên quyết; mới mở form đồ án |

M35-B/M36 chuyển sang đầu Năm 6 theo baseline 26/10/2026; nhóm asset theo module không phải ranh giới năm narrative. Reuse asset thi/portal hiện có.

### M5 — Năm 6

| Mission | R | N cụ thể | AN batch | U | Điều cần giữ trong flow |
|---|---|---|---|---|---|
| M37 | R_TEACHER, R_LAPTOP | luon_shirt_sit, advisor_office_background | AN02 | U_ACADEMIC, U_DIALOGUE | GVHD dùng visual teacher; đề tài mạng thống nhất xuyên năm6 |
| M38 | R_STUDY, R_LAPTOP, R_LUON | library_background | Không | U_RESEARCH | Tài liệu và graph bằngUI; không generate chữ/sơ đồ tĩnh |
| M39 | R_ROOM, R_TYPE, R_LAPTOP | Không | Không | U_CODE, U_TIMELAPSE, U_DEMO | Thường phục khi code đêm;22:43→05:26; sơmi chỉ cảnh gặp/bảo vệ |
| M40 | R_TEACHER, R_LAPTOP | luon_shirt_sit, advisor_office_background | AN02 | U_DEMO, U_DIALOGUE | Demo1 bị bác có feedback cụ thể; sau đó sửa và chạy test |
| M41 | R_TEACHER, R_LAPTOP, R_TYPE | luon_shirt_sit, thesis_book_blank, advisor_office_background | AN02 | U_CV_WORD, U_DEMO, U_INVENTORY | Demo2duyệt; vềphòng thường phục làm Word; không bake120trang |
| M42 | R_ROOM, R_LAPTOP, R_TYPE | Không | Không | U_CODE | Đề xuất22:45→23:45; submit mẫu23:44:32; có receipt/backup |
| M43 | R_TEACHER, R_GRADES | committee_chair, committee_examiner_a, committee_examiner_b, committee_examiner_c, luon_presenting, thesis_book_blank, podium, committee_table, projector_screen_blank, defense_room_background | AN02 | U_DEFENSE, U_DEMO | 5người=GVHD cũ+4giámkhảo mới; đạt mớiA; chưa nhận bằng vật lý |
| M44 | R_ROOM, R_HALL, R_BAGPROP, R_LAPTOP, R_SWITCH | npc_dorm_manager, npc_freshman_1, npc_freshman_2, npc_freshman_3, luon_phone_happy, luon_packing, keys_generic, screwdriver_kit, bedding_freshman_a, bedding_freshman_b, bedding_freshman_c, thesis_book_blank, dorm_reception_background | AN00, AN01, AN02 | U_PHONE, U_INVENTORY, U_ENDING, U_ROOM_STATE | Chỉ gócLươn trống; giữ3bedding mới; trảkhóa quầy; chưaFinished |
| M45 | R_DIPLOMA, R_DIALOGUE | long_office, to_office, dam_travel, father_luon, mother_luon, luon_reunion_arm, dam_reunion_arm, long_reunion_arm, to_reunion_arm, beer_supply, bouquet, graduation_backdrop_blank, beer_table, drinking_glass, a5_graduation_hall_background, binh_dong_night_background | AN00, AN03 | U_ENDING, U_TRAVEL, U_PHONE | Nhận bằng→đổi thường phục→BìnhĐông→khoácvai→credits; một backdrop cuối |

## 5. Quy cách chung và danh sách batch

- Thứ tự ô: **trái→phải, trên→dưới**, index bắt đầu 1. Crop trong JSON là **[x,y,width,height], gốc trên-trái**, không phải [left,top,right,bottom].
- C/P/AN cell 512 × 512, safe inset 32 px mỗi cạnh. Background cell 2048 × 1024, safe inset 64 px cho phần quan trọng. Không vẽ đường chia, số ô hay label.
- C/P/AN alpha transparent; nếu tool không giữ alpha dùng nền phẳng #FFFFFF hoặc #000000 tương phản rồi tách nền ở bước slicing. Background opaque.
- Không bake bóng sàn, chữ/giá/giờ/logo vào sprite. Shadow riêng; chữ tiếng Việt bằng TMP/UI.
- PPU100, Point, Uncompressed, no mipmaps theo baseline hiện tại. Pivot nhân vật/props đặt sàn bottom-center; background center. Item cầm tay giữ pivot mặc định rồi thêm grip anchor riêng.
- Nhân vật source giữ cùng tỉ lệ đầu/cơ thể và đường chân; không resize từng frame để vừa khung gây nhảy chân. Standing runtime target canvas 256 × 320, khoảng chiều cao thân tương đương Lươn cũ~233 px; feet baseline và scale thực tế phải ghi manifest sau khi đo. Pose ngồi giữ tỉ lệ cơ thể, không phóng to bằng pose đứng.
- Canvas là mục tiêu layout, không cam kết tool trả đúng resolution. Nếu source khác kích thước, đo grid thực tế, cập nhật actual_crop và chuẩn hóa đồng bộ; không áp tọa độ requested vào source khác resolution.
- Dùng reference cụ thể: Lươn `characters/luon_idle_0.png` + walk, ba bạn `*_standing.png`, Room 404 hiện có, H6 v 2 cho perspective. Scene mới theo palette/chibi đã dùng, không tự chuyển sang isometric hoặc realistic khác game.
- Cảnh phải chừa vùng đi bộ, marker cửa/ghế, lớp foreground. Vật thể tương tác/NPC không bake vào backdrop. Props tách rời không được lặp lại trong background.

| Batch | Loại | Grid | Canvas mục tiêu | Ô | Mission |
|---|---|---|---|---:|---|
| C00 | character | 4 × 2 | 2048 × 1024 | 8 | M01, M02, M04, M05, M06, M07, M17, M18, M19, M29, M30, M31, M33, M44 |
| C01 | character | 4 × 2 | 2048 × 1024 | 8 | M10, M11, M12, M14, M16, M19 |
| C02 | character | 4 × 2 | 2048 × 1024 | 8 | M17, M19, M20, M21, M22 |
| C03 | character | 4 × 2 | 2048 × 1024 | 8 | M24, M26, M28, M29, M30, M31, M34, M45 |
| C04 | character | 4 × 2 | 2048 × 1024 | 8 | M28, M43, M44, M45 |
| C05 | character | 3 × 2 | 1536 × 1024 | 6 | M37, M40, M41, M44, M45 |
| P00 | prop | 4 × 2 | 2048 × 1024 | 8 | M01, M03, M04, M05 |
| P01 | prop | 4 × 2 | 2048 × 1024 | 8 | M02, M07, M11, M12, M17, M18, M19, M33, M44 |
| P02 | prop | 4 × 2 | 2048 × 1024 | 8 | M03, M17, M18, M19, M20, M21, M44 |
| P03 | prop | 4 × 2 | 2048 × 1024 | 8 | M21, M22, M28, M29, M30, M31 |
| P04 | prop | 4 × 2 | 2048 × 1024 | 8 | M04, M08, M29, M30, M31, M33, M34, M41, M43, M44, M45 |
| P05 | prop | 3 × 2 | 1536 × 1024 | 6 | M08, M22, M34, M43, M45 |
| E00 | environment | 1 × 2 | 2048 × 2048 | 2 | M01, M02, M44 |
| E01 | environment | 1 × 2 | 2048 × 2048 | 2 | M04, M05, M22 |
| E02 | environment | 1 × 2 | 2048 × 2048 | 2 | M21, M24 |
| E03 | environment | 1 × 2 | 2048 × 2048 | 2 | M28, M37, M40, M41 |
| E04 | environment | 1 × 2 | 2048 × 2048 | 2 | M38, M43 |
| E05 | environment | 1 × 2 | 2048 × 2048 | 2 | M45 |
| AN00 | animation | 5 × 2 | 2560 × 1024 | 10 | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 |
| AN01 | animation | 5 × 2 | 2560 × 1024 | 10 | M01, M03, M06, M07, M19, M24, M33, M44 |
| AN02 | animation | 5 × 3 | 2560 × 1536 | 15 | M37, M40, M41, M43, M44 |
| AN03 | animation | 5 × 3 | 2560 × 1536 | 15 | M45 |
| AN04 | animation | 4 × 3 | 2048 × 1536 | 12 | M12, M29 |
| AN05 | animation | 4 × 3 | 2048 × 1536 | 12 | M29, M30, M31 |

## 6. Danh sách từng ô nhân vật, props và môi trường

### C00 — character, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `npc_dorm_manager` | M02, M44 | Cô quản lý đứng tại quầy; hai tay để nhận giấy/chìa khóa, không bake quầy vào người. |
| 2 | `npc_station_tout` | M01 | NPC mời xe đứng; tay ra hiệu; nhân bản bằng palette và vị trí cho đoạn né. |
| 3 | `npc_canteen_vendor` | M04, M05 | Người bán ở căn tin, pose đứng sau quầy. |
| 4 | `npc_h6_classmate_writing` | M06, M07 | Bạn cùng lớp Bách Khoa đang viết; thiết kế khác Đạm, ngồi đúng góc bàn H6. |
| 5 | `npc_repair_customer` | M17, M18, M19 | Sinh viên khách hàng đứng; laptop là prop tách rời. |
| 6 | `npc_freshman_1` | M07, M29, M33, M44 | Người mới 1, cùng độ tuổi nhưng nhận diện khác ba bạn cũ. |
| 7 | `npc_freshman_2` | M07, M30, M33, M44 | Người mới 2, phục trang khác người mới 1. |
| 8 | `npc_freshman_3` | M07, M31, M33, M44 | Người mới 3; không để ba giường thành chỉ hai người mới. |

### C01 — character, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `luon_tired_idle` | M10, M11, M19 | Lươn thường phục hiện có, mắt mệt và vai chùng, giữ nhận diện/chiều cao. |
| 2 | `luon_phone_sad` | M16 | Đứng nghe điện thoại buồn; điện thoại nằm trong bàn tay, không vẽ khung gọi. |
| 3 | `long_gaming_shout` | M10 | Long ngồi gaming hò hét, tai nghe, không bake laptop/ghế. |
| 4 | `luon_exam_worried` | M14 | Ngồi lo lắng, tay cầm bút, không bake bàn hoặc đề thi. |
| 5 | `luon_exam_bite_pen` | M14 | Cùng vị trí/pivot với worried; thay nét mặt và tay cắn bút. |
| 6 | `luon_carry_hotpot` | M12 | Lươn đỡ hai tay; không bake nồi để gắn nồi lẩu cũ ở anchor. |
| 7 | `guard_idle` | M12 | Bảo vệ đồng phục xanh; chìa khóa/đèn pin tách riêng. |
| 8 | `guard_inspecting` | M12 | Cùng bảo vệ, nghiêng nhìn kiểm tra; cùng scale với idle. |

### C02 — character, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `crush_idle` | M20, M21 | Nữ sinh đứng, ôm tài liệu; không tạo portrait riêng. |
| 2 | `crush_boyfriend_idle` | M21 | Nam sinh đứng, cùng tuổi; xe là prop rời. |
| 3 | `luon_repair_laptop` | M17, M19, M20, M21 | Lươn ngồi sửa, tay làm việc; laptop/bộ đồ nghề rời. |
| 4 | `luon_borrowed_jacket` | M21 | Lươn mặc áo khoác mượn Long, pose đứng tại quán; di chuyển đoạn ngắn bằng ghép trang phục nếu đạt QA. |
| 5 | `luon_lake_sit` | M05, M22 | Lươn ngồi ghế, chân đặt đất; góc nhìn nhóm Hồ Đá đã thống nhất. |
| 6 | `dam_lake_sit` | M05, M22 | Đạm ngồi ghế cùng góc, tay đưa nhẹ an ủi. |
| 7 | `long_lake_sit` | M05, M22 | Long ngồi ghế, tay cầm ly qua attachment, không bake ly. |
| 8 | `to_lake_sit` | M05, M22 | Tộ ngồi ghế, tay cầm đồ ăn qua attachment. |

### C03 — character, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `dam_lab_pose` | M24 | Đạm trong lab thảo luận/nghiên cứu, kính và nhận diện như sprite cũ. |
| 2 | `long_office` | M26, M29, M34, M45 | Long công sở, tay tự do để gắn vali/đồ tiếp tế. |
| 3 | `to_office` | M26, M30, M34, M45 | Tộ công sở, giữ nét nhận diện/thời trang riêng. |
| 4 | `dam_travel` | M31, M34, M45 | Đạm gọn gàng chuẩn bị đi xa; chưa bay trước M34. |
| 5 | `luon_carry_box` | M29 | Lươn tay đỡ thùng rời, đứng bắt đầu tương tác. |
| 6 | `dam_graduate` | M28 | Đạm áo tốt nghiệp, không logo/chữ tự tạo; đây là buổi chụp chung sau lễ riêng. |
| 7 | `long_graduate` | M28 | Long áo tốt nghiệp, đồng nhất cơ thể với sprite cũ. |
| 8 | `to_graduate` | M28 | Tộ áo tốt nghiệp, giữ tóc và phụ kiện nhận diện. |

### C04 — character, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `committee_chair` | M43 | Chủ tịch hội đồng ngồi, bàn là lớp tách rời. |
| 2 | `committee_examiner_a` | M43 | Giám khảo A ngồi cầm bút; nhận diện khác chủ tịch. |
| 3 | `committee_examiner_b` | M43 | Giám khảo B ngồi nhìn thí sinh. |
| 4 | `committee_examiner_c` | M43 | Giám khảo C ngồi xem tài liệu. |
| 5 | `luon_presenting` | M43 | Lươn sơ mi trắng/quần tây, tay remote; mặt hướng hội đồng, không bake bục. |
| 6 | `father_luon` | M28, M45 | Bố đứng chúc mừng, không bake Lươn/hoa vào cùng cell. |
| 7 | `mother_luon` | M28, M45 | Mẹ đứng chúc mừng, hoa gắn riêng để không che mặt. |
| 8 | `luon_phone_happy` | M44 | Lươn nghe điện thoại sau bảo vệ, vui xúc động, mặc sơ mi. |

### C05 — character, grid 3 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `luon_packing` | M44 | Lươn thường phục thu dọn góc riêng, đồ/balo tách rời. |
| 2 | `luon_shirt_sit` | M37, M40, M41 | Lươn sơ mi ngồi, dùng tại văn phòng/duyệt demo; giữ cùng trang phục bảo vệ. |
| 3 | `luon_reunion_arm` | M45 | Lươn thường phục, vị trí trái trong nhóm bốn, tay nối Đạm. |
| 4 | `dam_reunion_arm` | M45 | Đạm vị trí thứ hai, tay nối Lươn/Long. |
| 5 | `long_reunion_arm` | M45 | Long vị trí thứ ba, tay nối Đạm/Tộ. |
| 6 | `to_reunion_arm` | M45 | Tộ vị trí phải, tay nối Long; cùng hàng vai/feet line với ba ô trước. |

### P00 — prop, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `nem_chua_box` | M01, M03 | Thùng quà quê nhỏ, không nhãn chữ. |
| 2 | `cleaning_bucket` | M04 | Xô nước riêng, phối cảnh phòng hiện có. |
| 3 | `cleaning_rag` | M04 | Giẻ lau riêng; bản bẩn/sạch dùng tint. |
| 4 | `mini_fan` | M03 | Quạt con cóc nhỏ, dây điện có thể vẽ bằng line trong Unity. |
| 5 | `meal_tray` | M05 | Khay cơm trống các ô để gắn thức ăn. |
| 6 | `food_rice` | M05 | Một phần cơm riêng, cùng góc khay. |
| 7 | `food_meat` | M05 | Một món mặn riêng, không bake giá/chữ. |
| 8 | `food_vegetables` | M05, M08 | Một món xào riêng, cùng tỉ lệ món mặn/cơm. |

### P01 — prop, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `alarm_clock_blank` | M11, M33 | Đồng hồ mặt hiển thị trống; số giờ dựng runtime. |
| 2 | `wardrobe_closed` | M12 | Tủ đóng, cùng kích thước và phối cảnh với tủ mở. |
| 3 | `wardrobe_open_empty` | M12 | Tủ mở rỗng có vùng đủ đặt nồi cũ; nồi tách lớp, không generate bản tủ có nồi. |
| 4 | `keys_generic` | M02, M12, M44 | Chùm chìa khóa; chọn crop một chìa cho phòng 404, không generate riêng lần nữa. |
| 5 | `flashlight` | M12 | Đèn pin rời; tia sáng do Unity. |
| 6 | `toothbrush_cup` | M06, M07, M33 | Cốc và bàn chải, dùng tại WC. |
| 7 | `towel_folded` | M06, M07, M33 | Khăn gấp, dùng như prop; không cần frame giặt. |
| 8 | `usb_boot` | M17, M18, M19 | USB không logo; cắm/nhổ bằng transform. |

### P02 — prop, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `screwdriver_kit` | M03, M17, M19, M20, M21, M44 | Bộ tua vít mở nắp, một tua vít rời nằm cách bộ để có thể crop thêm sub-sprite từ cùng cell. |
| 2 | `laptop_open_case` | M18, M19, M21 | Laptop mở vỏ, góc sửa máy; màn hình/linh kiện riêng trong cùng vật thể. |
| 3 | `savings_jar` | M17, M19 | Hũ tiết kiệm trống chữ; lượng tiền thay bằng overlay. |
| 4 | `cash_notes_blank` | M17, M19 | Một nhóm tiền minh họa không số mệnh giá; số tiền hiển thị UI. |
| 5 | `service_sign_blank` | M18, M19 | Biển carton trống để đặt chữ CÀI WIN–SỬA LAPTOP404 bằng TMP. |
| 6 | `bread` | M19 | Bánh mì cầm tay. |
| 7 | `technician_bag` | M19 | Túi dụng cụ rời, gắn lên bộ walk hiện có nếu không xuyên hình. |
| 8 | `scooter_white` | M21 | Xe tay ga trắng dựng chân chống, không logo. |

### P03 — prop, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `lake_bench` | M22 | Ghế dài đủ bốn chỗ hoặc modular để lặp; không bake người. |
| 2 | `tea_food_cart_blank` | M21, M22 | Xe trà/đồ ăn không chữ, dùng chung quán và hồ. |
| 3 | `street_lamp` | M21, M22 | Đèn đường không bake quầng sáng. |
| 4 | `peach_tea` | M21, M22 | Ly trà đào rời; không bake tay. |
| 5 | `rice_paper_snack` | M22 | Đồ ăn rời; mở gói bằng UI/tween. |
| 6 | `moving_box_blank` | M29 | Thùng carton, chữ tên bạn dùng runtime. |
| 7 | `suitcase` | M29, M30, M31 | Vali chung, đổi palette cho ba bạn; không bake tay kéo. |
| 8 | `camera` | M28 | Máy ảnh rời, giao diện khung ngắm dựng Unity. |

### P04 — prop, grid 4 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `bedding_freshman_a` | M29, M33, M44 | Bộ chăn/gối người mới 1, đúng mặt giường cũ; giữ tới kết thúc. |
| 2 | `bedding_freshman_b` | M30, M33, M44 | Bộ chăn/gối người mới 2, khác hoa văn. |
| 3 | `bedding_freshman_c` | M31, M33, M44 | Bộ chăn/gối người mới 3, khác hai bộ trước. |
| 4 | `dried_buffalo_meat` | M04, M08, M30 | Thịt trâu quà Tộ, tái dùng nhiều năm. |
| 5 | `hotpot_supply_bag` | M34 | Túi tiếp tế, nồi lẩu cũ lấy ra theo tương tác. |
| 6 | `beer_supply` | M34, M45 | Bia/lốc đồ uống không logo; dùng lại bàn cuối. |
| 7 | `thesis_book_blank` | M41, M43, M44 | Bìa xanh thẫm trống chữ; title dựng runtime. |
| 8 | `bouquet` | M45 | Bó hoa rời cho mẹ/bạn/nhận bằng, đổi tint nếu cần. |

### P05 — prop, grid 3 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `podium` | M43 | Bục micro rời; mặt trước che chân theo occlusion. |
| 2 | `committee_table` | M43 | Bàn hội đồng đủ năm vị trí, không bake người/chữ. |
| 3 | `projector_screen_blank` | M43 | Màn chiếu trống; slide qua UI/texture runtime. |
| 4 | `graduation_backdrop_blank` | M45 | Phông lễ trống; tên lễ và trường dựng runtime. |
| 5 | `beer_table` | M45 | Bàn inox và đồ ăn không có người, ly và bia để rời nếu tương tác. |
| 6 | `drinking_glass` | M08, M22, M34, M45 | Ly dùng cụng; đồ uống đổi màu bằng overlay, không cần ảnh frame mới. |

### E00 — environment, grid 1 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `bus_station_background` | M01 | Bến xe và lối tới trạm; lối đi ngang rộng; không bake NPC/xe đang chạy/biển chữ. Dùng bus 33 cũ làm prop ở điểm đón. |
| 2 | `dorm_reception_background` | M02, M44 | Sảnh KTX có quầy trống và cầu thang/cửa ra; đủ chỗ nhận giấy và trả khóa. |

### E01 — environment, grid 1 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `canteen_background` | M04, M05 | Quầy mua đồ và khu bàn ăn; bàn nhóm đủ bốn chỗ, chừa người/khay tách rời. |
| 2 | `lake_background` | M22 | Hồ Đá chiều tối, mặt nước, nền cây và đường; chừa vùng đặt ghế bốn người/xe trà/đèn rời. |

### E02 — environment, grid 1 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `drink_cafe_background` | M21 | Quán nước có bàn sửa máy tạm, chừa xe tay ga; bố trí ba người không che mặt. |
| 2 | `research_lab_background` | M24 | Lab trường Tự Nhiên, bàn nghiên cứu trống người; tài liệu và máy móc phụ có thể bake nếu không tương tác. |

### E03 — environment, grid 1 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `vnu_photo_courtyard_background` | M28 | Sân chụp ảnh chung tại khu ĐHQG; chừa ba vị trí tốt nghiệp và người cầm máy; không chữ sai trường. |
| 2 | `advisor_office_background` | M37, M40, M41 | Văn phòng GVHD có bàn trống, ghế thầy/sinh viên; chừa laptop/báo cáo, không bake nhân vật. |

### E04 — environment, grid 1 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `library_background` | M38 | Thư viện có điểm tìm tài liệu và bàn sơ đồ; không bake người có tương tác. |
| 2 | `defense_room_background` | M43 | Phòng hội đồng, chừa năm ghế/bàn rời và vị trí thuyết trình/màn chiếu; không bake chữ và NPC. |

### E05 — environment, grid 1 × 2

| Ô | Asset ID | Mission | Mô tả / ràng buộc |
|---:|---|---|---|
| 1 | `a5_graduation_hall_background` | M45 | Sảnh/lễ A5, chừa backdrop, nhóm gia đình và điểm trao bằng; crowd phụ có thể bake ngoài vùng chơi. |
| 2 | `binh_dong_night_background` | M45 | Một cảnh rộng Bến Bình Đông có lan can, bờ kênh và vùng bàn nhậu/khoác vai; camera crop từ cùng ảnh, không tạo góc thứ hai. |

## 7. Animation đủ hướng và chuyển động

- Di chuyển gameplay giữ bốn hướng. Hiện chỉ có chu kỳ ngang thường phục và backpack; tái dùng hướng phải và flip trái khi phụ kiện đối xứng. AN00/01 bổ sung front/back.
- AN02/03 tạo front/back/right cho sơ mi và áo tốt nghiệp; trái là mirror phải khi QA xác nhận không đảo phụ kiện quan trọng. Không gọi một frame pose đứng là đủ animation di chuyển.
- Mỗi hướng locomotion gồm **idle +4 walk**, cùng canvas/feet line. Tốc độ mặc định 8 fps; idle không nhấp nháy. AN04/05 gồm 4 walk mỗi action/NPC; idle lấy C01/C03 tương ứng.
- Bảo vệ và bạn rời KTX có route ngang tới cửa; không đòi front/back cho NPC không đi theo trục đó. Freshmen/crush/khách đứng tại marker, vào cảnh qua transition thay vì lướt pose đứng.
- Balo/nồi/thùng/vali/túi là attachment. Tránh bake vật vào các frame rồi vô tình render thêm bản rời lần nữa.
- Mượn áo M21 và túi dụng cụ M19: thử ghép trang phục/attachment lên walk; nếu camera thấy sai, ghi phát sinh vào register điều kiện mục 10 trước khi thêm frame.
- M34 dùng pose sàn cũ. Nếu cần công sở khi ngồi, ghép vùng áo từ sprite công sở mới và pose sàn qua rig/mask; nếu QA không đạt, dùng batch điều kiện, không mặc định generate lại bộ lẩu.
- Hội ngộ: bốn pose C05 cùng row vai/anchor; render thử composite cả nhóm từ bản sheet trước khi tách duyệt. Không cần thêm một ảnh group ghép bake cả 4 người.

### AN00 — grid 5 × 2

| Hàng | Ô trái→phải |
|---:|---|
| 1 | `luon_casual_front_idle`; `luon_casual_front_walk_0`; `luon_casual_front_walk_1`; `luon_casual_front_walk_2`; `luon_casual_front_walk_3` |
| 2 | `luon_casual_back_idle`; `luon_casual_back_walk_0`; `luon_casual_back_walk_1`; `luon_casual_back_walk_2`; `luon_casual_back_walk_3` |

### AN01 — grid 5 × 2

| Hàng | Ô trái→phải |
|---:|---|
| 1 | `luon_backpack_front_idle`; `luon_backpack_front_walk_0`; `luon_backpack_front_walk_1`; `luon_backpack_front_walk_2`; `luon_backpack_front_walk_3` |
| 2 | `luon_backpack_back_idle`; `luon_backpack_back_walk_0`; `luon_backpack_back_walk_1`; `luon_backpack_back_walk_2`; `luon_backpack_back_walk_3` |

### AN02 — grid 5 × 3

| Hàng | Ô trái→phải |
|---:|---|
| 1 | `luon_shirt_front_idle`; `luon_shirt_front_walk_0`; `luon_shirt_front_walk_1`; `luon_shirt_front_walk_2`; `luon_shirt_front_walk_3` |
| 2 | `luon_shirt_back_idle`; `luon_shirt_back_walk_0`; `luon_shirt_back_walk_1`; `luon_shirt_back_walk_2`; `luon_shirt_back_walk_3` |
| 3 | `luon_shirt_right_idle`; `luon_shirt_right_walk_0`; `luon_shirt_right_walk_1`; `luon_shirt_right_walk_2`; `luon_shirt_right_walk_3` |

### AN03 — grid 5 × 3

| Hàng | Ô trái→phải |
|---:|---|
| 1 | `luon_robe_front_idle`; `luon_robe_front_walk_0`; `luon_robe_front_walk_1`; `luon_robe_front_walk_2`; `luon_robe_front_walk_3` |
| 2 | `luon_robe_back_idle`; `luon_robe_back_walk_0`; `luon_robe_back_walk_1`; `luon_robe_back_walk_2`; `luon_robe_back_walk_3` |
| 3 | `luon_robe_right_idle`; `luon_robe_right_walk_0`; `luon_robe_right_walk_1`; `luon_robe_right_walk_2`; `luon_robe_right_walk_3` |

### AN04 — grid 4 × 3

| Hàng | Ô trái→phải |
|---:|---|
| 1 | `guard_right_walk_0`; `guard_right_walk_1`; `guard_right_walk_2`; `guard_right_walk_3` |
| 2 | `luon_box_right_walk_0`; `luon_box_right_walk_1`; `luon_box_right_walk_2`; `luon_box_right_walk_3` |
| 3 | `luon_pot_right_walk_0`; `luon_pot_right_walk_1`; `luon_pot_right_walk_2`; `luon_pot_right_walk_3` |

### AN05 — grid 4 × 3

| Hàng | Ô trái→phải |
|---:|---|
| 1 | `long_leave_right_walk_0`; `long_leave_right_walk_1`; `long_leave_right_walk_2`; `long_leave_right_walk_3` |
| 2 | `to_leave_right_walk_0`; `to_leave_right_walk_1`; `to_leave_right_walk_2`; `to_leave_right_walk_3` |
| 3 | `dam_leave_right_walk_0`; `dam_leave_right_walk_1`; `dam_leave_right_walk_2`; `dam_leave_right_walk_3` |

GVHD ngồi hội đồng: crop phần đầu/thân trên của teacher vào ghế sau lớp bàn; đây là derived pose bằng rig/mask, không phải sprite ngồi đã có. Kiểm tra silhouette trước khi duyệt C04.

## 8. Asset dựng bằng Unity — không imagegen

| ID | Nội dung cần có |
|---|---|
| U_HUD | HUD ví/năng lượng/bromance/tín chỉ/giờ/ngày/kỳ; objective và prompt bàn phím/touch |
| U_DIALOGUE | Speaker/portrait/thoại/lựa chọn/next; crop portrait NPC từ sprite, font Việt |
| U_INVENTORY | Inventory, kéo thả giấy, nhận đồ, item attach/hand anchor; bàn giao chìa khóa |
| U_CLEAN | Bốn điểm bụi bằng mask/tint, timer 60 s, progress, retry không mua đồ lại |
| U_MEAL | Chọn món/giá/budget; slots khay cơm; xác nhận bữa ăn |
| U_TRAVEL | Zone/marker/camera/route, vé 7 k, điểm đón phụ/chuyến sau; trip/day reset |
| U_CLASS | Puzzle warm-up 1…10=55; lecture, hint 20 s/retry; cùng lớp là NPC mới |
| U_ACADEMIC | Portal, course graph/điểm/tín chỉ/cảnh cáo/đăng ký/điều kiện đồ án; academic card |
| U_PHONE | Điện thoại: nhóm 404, khách/crush, gọi mẹ, job, lịch thi/lễ và số dư |
| U_DESKTOP | Window giả lập chung: text/code/browser/forms/buttons/scroll; không ảnh OS tĩnh |
| U_TIMELAPSE | Clock/light/room state qua ngày/kỳ; montage và sổ thu chi |
| U_GAMING | YES/NO có nhánh hội tụ; màn game giả lập và âm loop |
| U_WAKE | Dậy/Snooze, alarm display, blur, flags điểm danh; hỗ trợ touch |
| U_STEALTH | 10 s, phích/nồi/tủ/chăn, vùng phát hiện, fail/retry; overlay cone đèn |
| U_CODE | Editor/log/patch/test/submit; M13, M39, M42 chung framework |
| U_EXAM | Đề/đáp án/feedback/tiến độ theo kiến thức; retry/assist; không ép 20% dù trả lời đúng |
| U_INSTALL | Boot/backup/chọn ổ/confirm/installer/Office giả lập/progress/reward |
| U_JOBS | Nhận/bàn giao job, cứu dữ liệu/vệ sinh/thiết lập mạng có đồng ý; ledger tiền và phí |
| U_REPAIR | Scan/quarantine/diagnostic/nguồn; checklist sửa máy |
| U_CV_WORD | Chỉnh font/lề/giãn dòng/mục lục/export/in; dùng chung CV và báo cáo |
| U_PHOTO | Viewfinder/flash/capture render scene; album có texture sinh runtime |
| U_ROOM_STATE | Room prefab nhiều trạng thái; NPC/đồ giường/biển/rác; góc Lươn dọn trống |
| U_UNINSTALL | Icon vector/menu/hold đúng nút/cancel/complete; touch giữ tương đương |
| U_RESEARCH | Tìm tài liệu, block graph, nối cổng, validate và nhật ký đề tài |
| U_DEMO | Demo thiết bị mạng, feedback bảo mật, kết quả test và task sửa |
| U_DEFENSE | Slide UI/chuyển slide/confidence/Q&A/score/retry, 5 người hội đồng |
| U_ENDING | Thu dọn, tắt đèn, trả khóa, nhận bằng, cụng ly/khoác vai, fade/credits/menu |
| U_COMMON | Menu/pause/settings/save/load/assist/audio; khóa input một chủ, load stable checkpoint |
| U_VFX | Steam/water/wind/glow/flash/fade/confetti bằng shader/particles/tween |

Mọi mission dùng thêm U_COMMON/U_HUD/U_DIALOGUE khi thích hợp, không cần lặp trong từng dòng mapping. Các sprite portrait NPC lấy crop từ cùng cell nhân vật, không gọi tạo ảnh mới. Sơ đồ/màn code/BIOS/Word/slide/UI nút giữ/đồng hồ hiển thị phải có trạng thái tương tác thật.

## 9. Data, room state, scene và audio

### D — nội dung/config

- D_QUEST_01–45: entry/action/exit, mục tiêu/trigger, item/cost/reward, success/fail/cancel/retry, checkpoint; không chỉ một enum nhảy năm.
- D_DIALOGUE: thoại nhập học/anh em/mẹ/crush/giáo sư/ba chia tay/tân sinh viên/hội ngộ; nhánh NO/dậy ngay/xin hỗ trợ có thoại riêng rồi hội tụ.
- D_COURSES: KTLT→CTDL→HĐH; GT2/VL1; điểm/tín chỉ từng học phần, học phí kỳ; không cộng credits hai lần. Mốc 32/50/78/118/136/150 là đề xuất cần cân bằng bằng tổng course ledger.
- D_JOBS: ba job đại diện, mạng/virus/vệ sinh/cài đặt, giá 50 k, nguồn chi/thu qua nhiều tuần; không yêu cầu grind hơn 60 máy.
- D_THESIS: tài liệu, graph/module, feedback demo 1, demo 2, lỗi/log/test, report 120 trang, Q&A dựa nội dung đã làm.
- D_TIMELINE: ngày/kỳ mới, lịch xe/điểm danh/thi/đồ án/lễ; M01 và M06 hai chuyến riêng.
- D_SAVE: quest/substep, flags, inventory, ledger, courses, roommate slot, room state, destination, choices. Lưu stable checkpoint; load không mất quyền điều khiển/nhân đôi tiền.
- D_AUDIO_CUES: cue theo scene/quest/light; pause/duck/fade và silence ở điểm F/cuộc gọi.

### Prefab/state và transition bắt buộc

| State/scene | Điều kiện mỹ thuật và gameplay |
|---|---|
| Opening | Bến xe→sảnhKTX→404; vé/giấy/chìa/balo có chủ sở hữu rõ |
| Room 404 lúc vào | Bụi 4 điểm; chưa có bạn; giường/bàn cùng perspective; claim marker |
| Room 404 sạch/anh em | Đủ 4 người; vị trí nghỉ/học/ăn; nồi chưa xuất hiện trước tương tác |
| Room 404 Năm 2 | Rác lớp rời tăng theo time-lapse; ngày/đêm và màn laptop không đổi ảnh nền |
| Room 404 dịch vụ | Biển chữ runtime; bàn sửa và job; không chặn đường tới giường/cửa |
| Chuyển giao | Long→Tộ→Đạm rời; từng slot giường đổi riêng, đủ 3 freshmen |
| Phục hồi | Góc học sạch; người mới vẫn có đồ, nhóm cũ đến tiếp tế rồi đi |
| M44 | Chỉ slot Lươn dọn; laptop/sách/tua vít vào balo; tắt đèn/ra cửa/trả khóa |
| M45 | A5 nhận bằng→đổi đồ/travel→Bình Đông; nhận bằng không xảy ra sớm tại M43 |

Mỗi transition có gate, lý do (tin nhắn/lịch/thoại), destination/spawn/facing/camera, khóa input một chủ, commit reward một lần, cập nhật ngày/state, restore physics/visibility/pose/UI/audio, cancel/load recovery. Hợp đồng chi tiết nằm trong FULL_FLOW_AUDIT_BEFORE_GENERATION.md.
Background mới cần walk bounds/collider/interaction marker/foreground derived mask; không phát sinh ảnh mới chỉ để chỉnh pivot hoặc scale. Quản lý KTX nhận lại khóa từ M44 trước khi tới ngày lễ M45.

### A — âm thanh/M6

| ID nhóm | Clip/cue |
|---|---|
| A_AMBIENCE | Bến xe, KTX/quạt, WC/nước, lớp, căn tin, lab/thư viện, phố, Hồ Đá/gió/nước, A5 đám đông, Bình Đông đêm |
| A_TRAVEL | Bước dép/gạch/cầu thang, balo/thùng/vali, xe buýt tới/đi, cửa sắt/khóa |
| A_STUDY | Bút/giấy/lật trang, typing/click, compile/test/error/submit |
| A_LIFE | Quạt điện, lau nhà, alarm/snooze, điện thoại rung/nhận/kết thúc, tiền/USB/tua vít |
| A_STEALTH | Chìa khóa bảo vệ, bước tuần tra, rút phích, nhấc nồi, mở/đóng tủ, timer |
| A_FRIENDS | Gõ cửa, lẩu sôi, gói đồ ăn, cụng ly, máy ảnh, chào tay/ôm qua foley |
| A_THESIS_END | Máy in, remote/projector, vỗ tay/trao bằng, switch tách, đóng cửa cuối |
| A_UI | Hover/confirm/cancel/error/progress complete, reward, lock/unlock; tái dùng cue chung |
| A_BGM | Sinh viên vui; gaming; sa sút; mưu sinh; Hồ Đá; chia ly; học/phục hồi; đồ án; bảo vệ; tốt nghiệp; acoustic credits |

Kiểm tra DormSound và clip đang có trước sản xuất audio. Voice-over không thuộc baseline. Mỗi clip có nguồn/quyền dùng/duration/loop/volume/cue; không dùng imagegen. M6 còn balance, font Việt, safe area/touch, build PC/mobile, performance và QA toàn tuyến.

## 10. Phát sinh có điều kiện — chưa nằm trong tổng ảnh baseline

Đây là phương án dự phòng, không phải lỗ hổng âm thầm trong số lượng. Chỉ kích hoạt nếu kiểm tra blockout/ghép asset không đạt; phải sửa JSON/count trước khi gọi tool.

| Điều kiện | Asset phát sinh | Layout khi thật sự cần |
|---|---|---|
| Mirror làm đảo phụ kiện quan trọng | Bộ left cho outfit bị ảnh hưởng: idle+4 walk | Gom các outfit cần sửa, grid 5 ×số outfit, cell 512 |
| Áo mượn M21 không ghép được vào walk | luon_jacket_right_walk_0–3 | Grid 4 × 1; route ngang trong quán; không tạo cả 4 hướng khi không dùng |
| Outfit công sở không ghép được pose sàn M34 | dam_office_floor,long_office_floor,to_office_floor | Grid 3 × 1, cell 512; dùng pot cũ |
| Tương tác ôm/chuyển bằng bị hở tay | Các pose contact cần thiết sau staging test | Gom ôm/trao/nhận theo cặp trong một sheet; chốt danh sách trước gọi tool |
| Căn tin thiếu món để thể hiện giới hạn 35 k | Một món đắt và một món thay thế có tác dụng gameplay | Grid 2 × 1, cùng góc khay; baseline 3 món vẫn đủ lượt chọn cơ bản |
| NPC/bối cảnh cũ không khớp phối cảnh sau thử ghép | Chỉ phần không sửa được bằng crop/rig/transform | Gộp với batch kế tiếp cùng loại, không generate ảnh lẻ |

Không thêm sprite để giải quyết lỗi quest graph, clock, save, input hoặc route.

## 11. Thực thi, slicing và nghiệm thu

### Hồ sơ mỗi batch

- Nguồn kế hoạch máy đọc: [plans/ASSET_BATCH_PLAN.json](plans/ASSET_BATCH_PLAN.json), chứa từng asset/cell/crop/mission/output/pivot/PPU và mapping 45 mission.
- Lưu `assets/mvp_sprites/remaining/<batch>/prompt.txt`, reference paths, source/final sheet, manifest thực tế, slice.py và preview. Không ghi source_file giả khi chưa generate.
- Output dự kiến `mobile-2 d/Assets/Sprites/MVP/remaining/<batch>/<asset-id>.png`; export .meta/import có pivot/PPU/filter; tên ID chưa phải reference script đã nối.
- Requested crop lấy từ grid. Actual crop đo từ source thực; nếu có divider/lệch panel, ghi tọa độ thật. Định dạng [x,y,w,h] thống nhất; ghi trim offset/scale/feet/grip anchors để slice lại được.
- Sprite actor chuẩn hóa cùng canvas/feet line, item có grip anchor, ghế có seat anchor; không tự crop alpha mỗi frame thành canvas kích thước khác.
- Tủ/bàn/ghế/giường/màn chiếu có foreground/mask từ crop cùng ảnh, không generate lại nền.
- Chỉ ghi generated batch khi thật sự sinh chung sheet. Ảnh cũ ghép contact sheet vẫn là ảnh cũ/repack, không đổi provenance.

### Thứ tự sản xuất

1. Blockout và kiểm tra mở đầu/45 mission/room states/ending bằng placeholder; khóa các marker/perspective trước batch ảnh liên quan.
2. Đợt 1 M1: C00, P00, E00, canteen ở E01; thêm AN00/AN01 khi triển khai di chuyển bốn hướng.
3. Đợt 2 M2: C01, P01, các row guard/pot trong AN04; reuse room/lẩu/rác/A/F.
4. Đợt 3 M3: C02, P02/P03, lake/cafe thuộc E01/E02; tránh regenerate panel đã đạt chỉ để gom lại đợt.
5. Đợt 4 M4: C03, P04, E02/E03 phù hợp, AN04 box và AN05.
6. Đợt 5 M5: C04/C05, P05, E03/E04/E05, AN02/AN03; lấy props/chăn đã có từ các đợt trước.
7. Audio/UI/data triển khai song song theo milestone bằng công cụ tương ứng; sau cùng M6 polish/balance/build.

Batch có panel dùng ở hai đợt vẫn generate **cả sheet một lần** khi bắt đầu sheet; thứ tự đợt là ưu tiên tích hợp, không được gọi tool từng panel. Có thể hoãn cả sheet tới khi cả hai panel đủ blockout.

### Checklist trước mỗi lệnh imagegen

- [ ] Asset không có bản cũ đáp ứng công dụng; đã thử crop/rig/scale/pivot khi phù hợp.
- [ ] Có ID/mission/cell và reference; blockout chốt camera/đường đi/anchor.
- [ ] Đủ số ô theo batch, không padding asset dư; prompt mô tả từng ô theo đúng thứ tự.
- [ ] Không bake text/UI, người/props tương tác vào background.
- [ ] Không thêm nhánh ending/scene/outfit ngoài baseline mà không cập nhật plan.

### QA sau sinh và khi tích hợp

- [ ] Không cắt đầu/tay/chân, không lẫn ô; alpha sạch, không bóng sàn/border/nhãn.
- [ ] Nhận diện nhân vật, palette, head/body ratio và outfit liên tục; từng hướng/frame đúng.
- [ ] Feet/pivot không nhảy; carry hands/props khớp; nhóm khoác vai chạm tay đúng.
- [ ] Background đúng perspective; layer trước/sau bàn/ghế/nồi và người khớp; đường đi không bị ảnh bake chặn.
- [ ] Credits/môn/giờ/tiền/thoại là runtime data; đủ font Việt và rõ trên mobile.
- [ ] Test mission entry→action→exit, retry/cancel/save/load; không kẹt input, trừ vé/cộng thưởng hai lần.
- [ ] M44 giữ đồ ba người mới; M45 mới đánh Finished sau đi cùng anh em.

## 12. Trạng thái và phần chưa được xác nhận

Đã hoàn thành **danh mục và kế hoạch sản xuất**; đang thực thi: asset ID, nhiệm vụ, batch/cell, animation, reuse, UI/data/audio, slicing và QA. Đã gọi imagegen cho các batch được ghi trong progress; đã thêm scene staging/import riêng; chưa xác nhận toàn tuyến qua Play Mode. Blockout và QA theo từng batch là bước thực thi của plan, không thể thay bằng việc tuyên bố mọi ảnh đều sẵn sàng tạo ngay.
Các mốc cân bằng và sửa flow trong mục 1 là baseline đề xuất cho plan này. Cần đồng bộ MASTER_GAME_FLOW/roadmap khi triển khai để không quay lại các mâu thuẫn đã ghi trong báo cáo audit.

## 13. Điều chỉnh qua kiểm tra thực tế

- C00/P00/E00 đã tạo theo batch và slicing; xem nhật ký thực thi, không xem đây là hoàn tất gameplay.
- Pose dam_sitting/long_sitting/to_sitting cũ đều ngồi xếp bằng trên sàn. M05 dùng chung bốn pose ghế C02 với M22 để tránh tạo thêm sheet; C02 phải sẵn sàng trước khi duyệt staging căn tin. Số lượng24sheet/178ô không đổi.

### Điều chỉnh normalization khi thực thi

- Nhóm khoác vai C05 dùng canvas384×320 để giữ scale cơ thể khi tay mở rộng; body/shoulder anchors và ảnh ghép nhóm được lưu trong C05. Không scale nhỏ nhân vật chỉ để ép tay vào256px.
- Props được trim vùng alpha, giữ scale source; cặp tủ dùng canvas đăng ký chung và pivot theo chân tủ để mở/đóng không nhảy vị trí.
- E05 thực tế là sheet rộng1774×887 với hai panel rất rộng; giữ aspect, đặt chiều cao scene8unit và camera crop thay vì kéo méo hoặc generate lại chỉ vì tỉ lệ. Gameplay staging cần duyệt lại với camera thật.

### Kế hoạch sửa animation sau Unity gallery

AN02: bốn frame đi ngang đều gần tư thế chân dang rộng, thiếu passing chân khép. Đổi thứ tự/crop/pivot không tạo được articulation còn thiếu. Refine **toàn sheet 5×3**: giữ hai hàng front/back và idle ngang; bốn ô walk ngang lần lượt contact chân gần → passing chân gần → contact chân xa → passing chân xa. Lưu nguồn cũ; đo lại crop, normalize chung scale/feet, duyệt loop và gameplay trước khi đạt motion QA. Chi tiết máy đọc trong `motion_corrections` của JSON. Đã refine toàn sheet và slice lại15ô; nguồn cũ lưu trong `AN02/before-gait-refinement/`. Static preview đạt; QA chuyển động trong gameplay vẫn chờ.

AN03 có cùng lỗi thiếu passing ở hướng ngang. Refine toàn sheet5×3, giữ front/back/idle và trang phục/mũ; bốn walk ngang xen kẽ contact/passing, hem robe đi theo chân. Lưu nguồn cũ, đo crop, giữ chiều cao270px gồm mũ và feet320; duyệt loop/gameplay sau slice.

### Nền tảng tiến độ production

Đã author45mission/212sự kiện theo baseline (M07 thêm ledger và từng cặp học/thi) và bổ sung model ví/course/inventory/checkpoint. Theo dõi [PRODUCTION_CAMPAIGN_IMPLEMENTATION.md](PRODUCTION_CAMPAIGN_IMPLEMENTATION.md) để biết phạm vi đã kiểm tra và thứ tự nối runtime. Đây chưa phải full route trong scene; các stage demo hiện có chưa được thay bằng graph này.

### Prototype mở đầu đã nối asset

Scene `Production_Opening` đã nối M01–M03 với 125 kiểm tra Play Mode qua, 0 lỗi. Dùng 5 output mới đã có và các asset cũ; thêm 9 bản căn khung từ sprite ngang cũ, không imagegen và không cộng vào 178 output chính. Xem [PRODUCTION_OPENING_REVIEW.md](PRODUCTION_OPENING_REVIEW.md). M04–M45 vẫn cần nối runtime; không đánh hoàn tất goal từ kết quả này.

### M04–M05 đã nối prototype

Đã dùng asset có sẵn cho mua xô/giẻ, bốn điểm dọn, ba bạn đến lần lượt và căn tin/menu/pose ghế. Ở giai đoạn M04–M05, lượt Play Mode bao phủ M01–M05 với271 kiểm tra qua, 0 lỗi. Không imagegen; thêm 3 standing căn khung từ ảnh cũ (12 bản dẫn xuất tổng cộng, ngoài 178 output chính). Xem [PRODUCTION_HOUSEKEEPING_REVIEW.md](PRODUCTION_HOUSEKEEPING_REVIEW.md). Tiếp theo M06–M08; M09–M45 và motion/device/audio/full-flow QA còn chờ.

### Adapter đi học — tiến độ triển khai, chưa nghiệm thu route M06–M08

- Đã tách `ICommuteProgress`/`ProductionCommuteProgress`: nguồn ví production duy nhất, đúng mission/step/choice/direction; không bind đồng thời campaign cũ.
- Xe commit checkpoint trước callback điểm đến trả quyền; lỗi save quay lại trạm, không trừ vé, có thể thử lại. WC có callback commit khi actor còn khóa; lưu thất bại không mở gate đã vệ sinh.
- Kiểm tra data/persistence host: `review/production-commute-host.json`; kiểm tra xe/WC thật trong fixture runtime: `review/commute-playmode/report.json`. Phạm vi này chưa chứng minh M01→M09 chơi xuyên suốt.
- Campaign năm nhất cũ đã đạt514 checks trước các adapter mới; text recovery có kiểm tra riêng117 assertions. Còn nối điểm lấy balo, hành lang/WC/trạm/H6/lớp/xe về, presentation bài học và lẩu vào production; không tự cấp32 tín chỉ sau warm-up hoặc nhảy gaming khi hếtM08.
- Không phát sinh batch ảnh mới. Tiếp tục tái dùng bus33, hallway/WC/H6/pot/floor và C00 classmate theo hợp đồng `PRODUCTION_SCHOOL_ADAPTER_CONTRACT.md`.

### M06 đã nối production — cập nhật sau Play Mode13:40UTC

- Scene production có8vùng và24action anchors, thêm hành lang/WC/trạm33/H6/lớp học. Đi balo đúng giường → WC → hai lượt xe7k → lớp/câu hỏi55 → về404/kể Đạm → M07.
- Lượt full M01–M06 qua cả hai lựa chọn giường đạt **390checks,0errors**. Model mới đạt346.807assertions/192tổ hợp và kiểm tra tương thích prefixM01–M05. Không cấp32tín chỉ ở warm-up; hoàn tấtM06 vẫn0credits.
- Đã sửa collider WC, sàn hành lang, đường đi giữa lớp, thứ tự renderer và tư thế ngồi. Asset có sẵn được dùng lại; không generate/slice thêm. Bằng chứng149file có hash: `review/opening-runtime-evidence.json`; chi tiết và giới hạn: [PRODUCTION_SCHOOL_REVIEW.md](PRODUCTION_SCHOOL_REVIEW.md).
- M07–M45, montage/ledger/tín chỉ năm1, lẩuM08→đăng kýM09, audio/motion/device/build/fullplaythrough còn pending. Chuyến vềM06 hiện trình bày đường qua KTX/cầu thang/phòng trong transition; chưa là đoạn cầu thang điều khiển tự do.

### M07–M08 đã nối gameplay prototype — bản xác nhận14:43UTC

- FullM01–M08qua cả hai giường và hai kết quả đón xe đạt **739checks/0errors**. Bản cuối xử lý cả actor bị hủy khi controller còn update và lúc dỡ scene; fixture lifecycle52checks. Model hiện45mission/212event,369.301assertions; timer/curriculum42assertions; route riêng438checksbao gồm xe đã đỗ rồi hết giờ.
- M07 có queueWC8s, clock gameplay120s lưu theo prefix (pause/reload không reset), xe chuyến sau thật, phản ứng điểm danh, ledger trích quỹ9vé/63k,6cặp bài học/thi với credits6/11/16/21/26/32và checkpoint giữa năm. Hai học kỳ có cầu nối; không cấp32chỉ sau warmup.
- M08: lấy nồi→giữ1sđể cắm điện ở đầu dây bên phải→thêm nguyên liệu góp→ăn4floor poses→cụng ly→đúng giường/ngủ→M09registration. Không gọi gaming demo; ví, credits và trạng thái chưaFinished giữ đúng.
- [PRODUCTION_YEAR_ONE_REVIEW.md](PRODUCTION_YEAR_ONE_REVIEW.md) ghi rõ scope. `review/opening-runtime-evidence.json` hiện261file cóhash, supersede snapshot149file của giai đoạnM06. Data/capture/source hiện tại được kiểm tra đồng bộ.
- Art detail còn4attachment đã có ảnh nhưng chưa bind: `toothbrush_cup`, `towel_folded`, `dried_buffalo_meat`, `drinking_glass`. WC hiện dùng door/wash/meter; floor sprites đã bake ly. Cần bind hoặc chốt lý do tái sử dụng trước nghiệm thu art cuối, không generate thêm để lấp chỗ. P00food_meat/vegetables được dùng lại ởM08, classmate ởM07; mapping đã cập nhật.
- M09–M45, audio/artistic motion/device/build/full-ending QA còn pending. Đây không phải hoàn thành mục tiêu toàn45mission.

### M09 và props đã tích hợp — bản xác nhận15:21UTC

FullM01–M09đạt819checks/0errors; M09riêng87checks. Snapshot292file cóhash. Cổng F5đúng giờ,3lớp bắt buộc/lớp muộn hết chỗ,draft chưa chốt không tự lưu,xác nhận lịch06:45và thẻ tuần học mởM10; ví và32credits giữ nguyên. Definition212event không đổi.

Cốc/bàn chải và khănP01đã bind vào progressWC; P04thịt trâu thay thịt chungM08; P05ly đặt cạnh nhóm. Không generate/slice thêm. To vẫn cầm bát trong floor pose cũ nên contact/đưa ly vào tay/cụng ly còn pending; không ghi đã duyệt animation. M10entry tint còn matte sáng ở góc nền, xử lý trong pass night scene bằng asset hiện có.

Xem PRODUCTION_REGISTRATION_REVIEW.md. M10–M45và gate cuối còn pending. Từ đây dùng kiểm tra riêng cho thay đổi cục bộ, fullroute ở mốc milestone/shared-flow; không chạy lại toàn tuyến chỉ vì copy/layout nhỏ.
