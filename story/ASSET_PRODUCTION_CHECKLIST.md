# Checklist asset đầy đủ và thứ tự sản xuất — Phòng 404

Cập nhật 25/10/2026. Nguồn máy đọc: [ASSET_BATCH_PLAN.json](plans/ASSET_BATCH_PLAN.json).

Danh sách người dùng dán là dự toán M2–M5, chưa đủ cho các milestone còn lại của toàn game. Danh mục chuẩn dưới đây bao phủ M01–M45. ID là tên file/catalog dự kiến, không có nghĩa đã nối vào script gameplay.

## Quyết định trước generate bổ sung — 25/10/2026

Danh sách từng ô bên dưới là baseline đầy đủ: **24 sheet / 178 asset-frame**. Hiện chưa lên lịch generate thêm. Trạng thái triển khai và các phần còn thiếu được chốt ở [plan tổng](REMAINING_ASSET_INVENTORY.md#chốt-phạm-vi-trước-lượt-generate-tiếp-theo--25102026) và `pre_generation_review` trong JSON. M01–M09 là gameplay prototype đã kiểm tra819checks; props WC/lẩu đã bind, còn contact/motion và M10–M45. Chi tiết tại PRODUCTION_YEAR_ONE_REVIEW.md. Mỗi phát sinh phải có lỗi quan sát, thử reuse, danh sách ô và thông số slicing trước khi tạo ảnh.

## Trạng thái và quy tắc tiếp tục

- 24 sheet/178 output đã có trong workspace; không generate lại các asset này chỉ vì cập nhật plan.
- Có file/source hoặc đã slice không đồng nghĩa đã đạt animation QA, staging hay gameplay.
- Trước bất kỳ lượt generate bổ sung/refine nào: ghi lỗi cụ thể, thử tái sử dụng/crop/pivot/rig, cập nhật danh sách ô và layout, rồi mới gọi tool.
- Thứ tự ô từ trái sang phải, từ trên xuống dưới. Grid là kế hoạch; crop thực tế trong JSON/manifest được ưu tiên khi slicing.
- Checklist này là danh mục sản xuất. Tiến độ từng output lấy từ JSON; nghiệm thu cuối cần full playthrough.

## Tổng khối lượng

| Nhóm | Số lượng |
|---|---:|
| Pose/NPC | 46 |
| Props | 46 |
| Bối cảnh | 12 |
| Frame animation | 74 |
| Tổng output mới | 178 |
| Sheet | 24 |
| Nhóm tái sử dụng | 27 |
| Nhóm Unity UI/hệ thống | 29 |

## Từng sheet và asset/frame

### C00 — character — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `npc_dorm_manager` | M02, M44 | Cô quản lý đứng tại quầy; hai tay để nhận giấy/chìa khóa, không bake quầy vào người. |
| 2 | `npc_station_tout` | M01 | NPC mời xe đứng; tay ra hiệu; nhân bản bằng palette và vị trí cho đoạn né. |
| 3 | `npc_canteen_vendor` | M04, M05 | Người bán ở căn tin, pose đứng sau quầy. |
| 4 | `npc_h6_classmate_writing` | M06, M07 | Bạn cùng lớp Bách Khoa đang viết; thiết kế khác Đạm, ngồi đúng góc bàn H6. |
| 5 | `npc_repair_customer` | M17, M18, M19 | Sinh viên khách hàng đứng; laptop là prop tách rời. |
| 6 | `npc_freshman_1` | M07, M29, M33, M44 | Người mới 1, cùng độ tuổi nhưng nhận diện khác ba bạn cũ. |
| 7 | `npc_freshman_2` | M07, M30, M33, M44 | Người mới 2, phục trang khác người mới 1. |
| 8 | `npc_freshman_3` | M07, M31, M33, M44 | Người mới 3; không để ba giường thành chỉ hai người mới. |

### C01 — character — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `luon_tired_idle` | M10, M11, M19 | Lươn thường phục hiện có, mắt mệt và vai chùng, giữ nhận diện/chiều cao. |
| 2 | `luon_phone_sad` | M16 | Đứng nghe điện thoại buồn; điện thoại nằm trong bàn tay, không vẽ khung gọi. |
| 3 | `long_gaming_shout` | M10 | Long ngồi gaming hò hét, tai nghe, không bake laptop/ghế. |
| 4 | `luon_exam_worried` | M14 | Ngồi lo lắng, tay cầm bút, không bake bàn hoặc đề thi. |
| 5 | `luon_exam_bite_pen` | M14 | Cùng vị trí/pivot với worried; thay nét mặt và tay cắn bút. |
| 6 | `luon_carry_hotpot` | M12 | Lươn đỡ hai tay; không bake nồi để gắn nồi lẩu cũ ở anchor. |
| 7 | `guard_idle` | M12 | Bảo vệ đồng phục xanh; chìa khóa/đèn pin tách riêng. |
| 8 | `guard_inspecting` | M12 | Cùng bảo vệ, nghiêng nhìn kiểm tra; cùng scale với idle. |

### C02 — character — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `crush_idle` | M20, M21 | Nữ sinh đứng, ôm tài liệu; không tạo portrait riêng. |
| 2 | `crush_boyfriend_idle` | M21 | Nam sinh đứng, cùng tuổi; xe là prop rời. |
| 3 | `luon_repair_laptop` | M17, M19, M20, M21 | Lươn ngồi sửa, tay làm việc; laptop/bộ đồ nghề rời. |
| 4 | `luon_borrowed_jacket` | M21 | Lươn mặc áo khoác mượn Long, pose đứng tại quán; di chuyển đoạn ngắn bằng ghép trang phục nếu đạt QA. |
| 5 | `luon_lake_sit` | M05, M22 | Lươn ngồi ghế, chân đặt đất; góc nhìn nhóm Hồ Đá đã thống nhất. Dùng chung pose ghế ở M05 và M22, tay cầm props qua attachment. |
| 6 | `dam_lake_sit` | M05, M22 | Đạm ngồi ghế cùng góc, tay đưa nhẹ an ủi. Dùng chung pose ghế ở M05 và M22, tay cầm props qua attachment. |
| 7 | `long_lake_sit` | M05, M22 | Long ngồi ghế, tay cầm ly qua attachment, không bake ly. Dùng chung pose ghế ở M05 và M22, tay cầm props qua attachment. |
| 8 | `to_lake_sit` | M05, M22 | Tộ ngồi ghế, tay cầm đồ ăn qua attachment. Dùng chung pose ghế ở M05 và M22, tay cầm props qua attachment. |

### C03 — character — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `dam_lab_pose` | M24 | Đạm trong lab thảo luận/nghiên cứu, kính và nhận diện như sprite cũ. |
| 2 | `long_office` | M26, M29, M34, M45 | Long công sở, tay tự do để gắn vali/đồ tiếp tế. |
| 3 | `to_office` | M26, M30, M34, M45 | Tộ công sở, giữ nét nhận diện/thời trang riêng. |
| 4 | `dam_travel` | M31, M34, M45 | Đạm gọn gàng chuẩn bị đi xa; chưa bay trước M34. |
| 5 | `luon_carry_box` | M29 | Lươn tay đỡ thùng rời, đứng bắt đầu tương tác. |
| 6 | `dam_graduate` | M28 | Đạm áo tốt nghiệp, không logo/chữ tự tạo; đây là buổi chụp chung sau lễ riêng. |
| 7 | `long_graduate` | M28 | Long áo tốt nghiệp, đồng nhất cơ thể với sprite cũ. |
| 8 | `to_graduate` | M28 | Tộ áo tốt nghiệp, giữ tóc và phụ kiện nhận diện. |

### C04 — character — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `committee_chair` | M43 | Chủ tịch hội đồng ngồi, bàn là lớp tách rời. |
| 2 | `committee_examiner_a` | M43 | Giám khảo A ngồi cầm bút; nhận diện khác chủ tịch. |
| 3 | `committee_examiner_b` | M43 | Giám khảo B ngồi nhìn thí sinh. |
| 4 | `committee_examiner_c` | M43 | Giám khảo C ngồi xem tài liệu. |
| 5 | `luon_presenting` | M43 | Lươn sơ mi trắng/quần tây, tay remote; mặt hướng hội đồng, không bake bục. |
| 6 | `father_luon` | M28, M45 | Bố đứng chúc mừng, không bake Lươn/hoa vào cùng cell. |
| 7 | `mother_luon` | M28, M45 | Mẹ đứng chúc mừng, hoa gắn riêng để không che mặt. |
| 8 | `luon_phone_happy` | M44 | Lươn nghe điện thoại sau bảo vệ, vui xúc động, mặc sơ mi. |

### C05 — character — grid 3 × 2

Canvas kế hoạch: 1536 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `luon_packing` | M44 | Lươn thường phục thu dọn góc riêng, đồ/balo tách rời. |
| 2 | `luon_shirt_sit` | M37, M40, M41 | Lươn sơ mi ngồi, dùng tại văn phòng/duyệt demo; giữ cùng trang phục bảo vệ. |
| 3 | `luon_reunion_arm` | M45 | Lươn thường phục, vị trí trái trong nhóm bốn, tay nối Đạm. |
| 4 | `dam_reunion_arm` | M45 | Đạm vị trí thứ hai, tay nối Lươn/Long. |
| 5 | `long_reunion_arm` | M45 | Long vị trí thứ ba, tay nối Đạm/Tộ. |
| 6 | `to_reunion_arm` | M45 | Tộ vị trí phải, tay nối Long; cùng hàng vai/feet line với ba ô trước. |

### P00 — prop — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `nem_chua_box` | M01, M03 | Thùng quà quê nhỏ, không nhãn chữ. |
| 2 | `cleaning_bucket` | M04 | Xô nước riêng, phối cảnh phòng hiện có. |
| 3 | `cleaning_rag` | M04 | Giẻ lau riêng; bản bẩn/sạch dùng tint. |
| 4 | `mini_fan` | M03 | Quạt con cóc nhỏ, dây điện có thể vẽ bằng line trong Unity. |
| 5 | `meal_tray` | M05 | Khay cơm trống các ô để gắn thức ăn. |
| 6 | `food_rice` | M05 | Một phần cơm riêng, cùng góc khay. |
| 7 | `food_meat` | M05, M08 | Một món mặn riêng, không bake giá/chữ. |
| 8 | `food_vegetables` | M05, M08 | Một món xào riêng, cùng tỉ lệ món mặn/cơm. |

### P01 — prop — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `alarm_clock_blank` | M11, M33 | Đồng hồ mặt hiển thị trống; số giờ dựng runtime. |
| 2 | `wardrobe_closed` | M12 | Tủ đóng, cùng kích thước và phối cảnh với tủ mở. |
| 3 | `wardrobe_open_empty` | M12 | Tủ mở rỗng có vùng đủ đặt nồi cũ; nồi tách lớp, không generate bản tủ có nồi. |
| 4 | `keys_generic` | M02, M12, M44 | Chùm chìa khóa; chọn crop một chìa cho phòng404, không generate riêng lần nữa. |
| 5 | `flashlight` | M12 | Đèn pin rời; tia sáng do Unity. |
| 6 | `toothbrush_cup` | M07, M33 | Cốc và bàn chải, dùng tại WC. |
| 7 | `towel_folded` | M07, M33 | Khăn gấp, dùng như prop; không cần frame giặt. |
| 8 | `usb_boot` | M17, M18, M19 | USB không logo; cắm/nhổ bằng transform. |

### P02 — prop — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `screwdriver_kit` | M03, M17, M19, M20, M21, M44 | Bộ tua vít mở nắp, một tua vít rời nằm cách bộ để có thể crop thêm sub-sprite từ cùng cell. |
| 2 | `laptop_open_case` | M18, M19, M21 | Laptop mở vỏ, góc sửa máy; màn hình/linh kiện riêng trong cùng vật thể. |
| 3 | `savings_jar` | M17, M19 | Hũ tiết kiệm trống chữ; lượng tiền thay bằng overlay. |
| 4 | `cash_notes_blank` | M17, M19 | Một nhóm tiền minh họa không số mệnh giá; số tiền hiển thị UI. |
| 5 | `service_sign_blank` | M18, M19 | Biển carton trống để đặt chữ CÀI WIN–SỬA LAPTOP404 bằng TMP. |
| 6 | `bread` | M19 | Bánh mì cầm tay. |
| 7 | `technician_bag` | M19 | Túi dụng cụ rời, gắn lên bộ walk hiện có nếu không xuyên hình. |
| 8 | `scooter_white` | M21 | Xe tay ga trắng dựng chân chống, không logo. |

### P03 — prop — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `lake_bench` | M22 | Ghế dài đủ bốn chỗ hoặc modular để lặp; không bake người. |
| 2 | `tea_food_cart_blank` | M21, M22 | Xe trà/đồ ăn không chữ, dùng chung quán và hồ. |
| 3 | `street_lamp` | M21, M22 | Đèn đường không bake quầng sáng. |
| 4 | `peach_tea` | M21, M22 | Ly trà đào rời; không bake tay. |
| 5 | `rice_paper_snack` | M22 | Đồ ăn rời; mở gói bằng UI/tween. |
| 6 | `moving_box_blank` | M29 | Thùng carton, chữ tên bạn dùng runtime. |
| 7 | `suitcase` | M29, M30, M31 | Vali chung, đổi palette cho ba bạn; không bake tay kéo. |
| 8 | `camera` | M28 | Máy ảnh rời, giao diện khung ngắm dựng Unity. |

### P04 — prop — grid 4 × 2

Canvas kế hoạch: 2048 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `bedding_freshman_a` | M29, M33, M44 | Bộ chăn/gối người mới1, đúng mặt giường cũ; giữ tới kết thúc. |
| 2 | `bedding_freshman_b` | M30, M33, M44 | Bộ chăn/gối người mới2, khác hoa văn. |
| 3 | `bedding_freshman_c` | M31, M33, M44 | Bộ chăn/gối người mới3, khác hai bộ trước. |
| 4 | `dried_buffalo_meat` | M04, M08, M30 | Thịt trâu quà Tộ, tái dùng nhiều năm. |
| 5 | `hotpot_supply_bag` | M34 | Túi tiếp tế, nồi lẩu cũ lấy ra theo tương tác. |
| 6 | `beer_supply` | M34, M45 | Bia/lốc đồ uống không logo; dùng lại bàn cuối. |
| 7 | `thesis_book_blank` | M41, M43, M44 | Bìa xanh thẫm trống chữ; title dựng runtime. |
| 8 | `bouquet` | M45 | Bó hoa rời cho mẹ/bạn/nhận bằng, đổi tint nếu cần. |

### P05 — prop — grid 3 × 2

Canvas kế hoạch: 1536 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `podium` | M43 | Bục micro rời; mặt trước che chân theo occlusion. |
| 2 | `committee_table` | M43 | Bàn hội đồng đủ năm vị trí, không bake người/chữ. |
| 3 | `projector_screen_blank` | M43 | Màn chiếu trống; slide qua UI/texture runtime. |
| 4 | `graduation_backdrop_blank` | M45 | Phông lễ trống; tên lễ và trường dựng runtime. |
| 5 | `beer_table` | M45 | Bàn inox và đồ ăn không có người, ly và bia để rời nếu tương tác. |
| 6 | `drinking_glass` | M08, M22, M34, M45 | Ly dùng cụng; đồ uống đổi màu bằng overlay, không cần ảnh frame mới. |

### E00 — environment — grid 1 × 2

Canvas kế hoạch: 2048 × 2048 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `bus_station_background` | M01 | Bến xe và lối tới trạm; lối đi ngang rộng; không bake NPC/xe đang chạy/biển chữ. Dùng bus33 cũ làm prop ở điểm đón. |
| 2 | `dorm_reception_background` | M02, M44 | Sảnh KTX có quầy trống và cầu thang/cửa ra; đủ chỗ nhận giấy và trả khóa. |

### E01 — environment — grid 1 × 2

Canvas kế hoạch: 2048 × 2048 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `canteen_background` | M04, M05 | Quầy mua đồ và khu bàn ăn; bàn nhóm đủ bốn chỗ, chừa người/khay tách rời. |
| 2 | `lake_background` | M22 | Hồ Đá chiều tối, mặt nước, nền cây và đường; chừa vùng đặt ghế bốn người/xe trà/đèn rời. |

### E02 — environment — grid 1 × 2

Canvas kế hoạch: 2048 × 2048 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `drink_cafe_background` | M21 | Quán nước có bàn sửa máy tạm, chừa xe tay ga; bố trí ba người không che mặt. |
| 2 | `research_lab_background` | M24 | Lab trường Tự Nhiên, bàn nghiên cứu trống người; tài liệu và máy móc phụ có thể bake nếu không tương tác. |

### E03 — environment — grid 1 × 2

Canvas kế hoạch: 2048 × 2048 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `vnu_photo_courtyard_background` | M28 | Sân chụp ảnh chung tại khu ĐHQG; chừa ba vị trí tốt nghiệp và người cầm máy; không chữ sai trường. |
| 2 | `advisor_office_background` | M37, M40, M41 | Văn phòng GVHD có bàn trống, ghế thầy/sinh viên; chừa laptop/báo cáo, không bake nhân vật. |

### E04 — environment — grid 1 × 2

Canvas kế hoạch: 2048 × 2048 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `library_background` | M38 | Thư viện có điểm tìm tài liệu và bàn sơ đồ; không bake người có tương tác. |
| 2 | `defense_room_background` | M43 | Phòng hội đồng, chừa năm ghế/bàn rời và vị trí thuyết trình/màn chiếu; không bake chữ và NPC. |

### E05 — environment — grid 1 × 2

Canvas kế hoạch: 2048 × 2048 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `a5_graduation_hall_background` | M45 | Sảnh/lễ A5, chừa backdrop, nhóm gia đình và điểm trao bằng; crowd phụ có thể bake ngoài vùng chơi. |
| 2 | `binh_dong_night_background` | M45 | Một cảnh rộng Bến Bình Đông có lan can, bờ kênh và vùng bàn nhậu/khoác vai; camera crop từ cùng ảnh, không tạo góc thứ hai. |

### AN00 — animation — grid 5 × 2

Canvas kế hoạch: 2560 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `luon_casual_front_idle` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: front, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 2 | `luon_casual_front_walk_0` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: front, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 3 | `luon_casual_front_walk_1` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: front, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 4 | `luon_casual_front_walk_2` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: front, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 5 | `luon_casual_front_walk_3` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: front, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 6 | `luon_casual_back_idle` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: back, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 7 | `luon_casual_back_walk_0` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: back, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 8 | `luon_casual_back_walk_1` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: back, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 9 | `luon_casual_back_walk_2` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: back, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 10 | `luon_casual_back_walk_3` | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M14, M16, M19, M21, M22, M24, M28, M29, M30, M31, M33, M35, M44, M45 | luon_casual: back, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |

### AN01 — animation — grid 5 × 2

Canvas kế hoạch: 2560 × 1024 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `luon_backpack_front_idle` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: front, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 2 | `luon_backpack_front_walk_0` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: front, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 3 | `luon_backpack_front_walk_1` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: front, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 4 | `luon_backpack_front_walk_2` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: front, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 5 | `luon_backpack_front_walk_3` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: front, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 6 | `luon_backpack_back_idle` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: back, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 7 | `luon_backpack_back_walk_0` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: back, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 8 | `luon_backpack_back_walk_1` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: back, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 9 | `luon_backpack_back_walk_2` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: back, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 10 | `luon_backpack_back_walk_3` | M01, M03, M06, M07, M19, M24, M33, M44 | luon_backpack: back, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |

### AN02 — animation — grid 5 × 3

Canvas kế hoạch: 2560 × 1536 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `luon_shirt_front_idle` | M37, M40, M41, M43, M44 | luon_shirt: front, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 2 | `luon_shirt_front_walk_0` | M37, M40, M41, M43, M44 | luon_shirt: front, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 3 | `luon_shirt_front_walk_1` | M37, M40, M41, M43, M44 | luon_shirt: front, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 4 | `luon_shirt_front_walk_2` | M37, M40, M41, M43, M44 | luon_shirt: front, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 5 | `luon_shirt_front_walk_3` | M37, M40, M41, M43, M44 | luon_shirt: front, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 6 | `luon_shirt_back_idle` | M37, M40, M41, M43, M44 | luon_shirt: back, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 7 | `luon_shirt_back_walk_0` | M37, M40, M41, M43, M44 | luon_shirt: back, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 8 | `luon_shirt_back_walk_1` | M37, M40, M41, M43, M44 | luon_shirt: back, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 9 | `luon_shirt_back_walk_2` | M37, M40, M41, M43, M44 | luon_shirt: back, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 10 | `luon_shirt_back_walk_3` | M37, M40, M41, M43, M44 | luon_shirt: back, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 11 | `luon_shirt_right_idle` | M37, M40, M41, M43, M44 | luon_shirt: right, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 12 | `luon_shirt_right_walk_0` | M37, M40, M41, M43, M44 | luon_shirt: right, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 13 | `luon_shirt_right_walk_1` | M37, M40, M41, M43, M44 | luon_shirt: right, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 14 | `luon_shirt_right_walk_2` | M37, M40, M41, M43, M44 | luon_shirt: right, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 15 | `luon_shirt_right_walk_3` | M37, M40, M41, M43, M44 | luon_shirt: right, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |

### AN03 — animation — grid 5 × 3

Canvas kế hoạch: 2560 × 1536 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `luon_robe_front_idle` | M45 | luon_robe: front, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 2 | `luon_robe_front_walk_0` | M45 | luon_robe: front, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 3 | `luon_robe_front_walk_1` | M45 | luon_robe: front, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 4 | `luon_robe_front_walk_2` | M45 | luon_robe: front, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 5 | `luon_robe_front_walk_3` | M45 | luon_robe: front, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 6 | `luon_robe_back_idle` | M45 | luon_robe: back, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 7 | `luon_robe_back_walk_0` | M45 | luon_robe: back, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 8 | `luon_robe_back_walk_1` | M45 | luon_robe: back, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 9 | `luon_robe_back_walk_2` | M45 | luon_robe: back, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 10 | `luon_robe_back_walk_3` | M45 | luon_robe: back, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 11 | `luon_robe_right_idle` | M45 | luon_robe: right, idle; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 12 | `luon_robe_right_walk_0` | M45 | luon_robe: right, walk_0; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 13 | `luon_robe_right_walk_1` | M45 | luon_robe: right, walk_1; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 14 | `luon_robe_right_walk_2` | M45 | luon_robe: right, walk_2; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |
| 15 | `luon_robe_right_walk_3` | M45 | luon_robe: right, walk_3; cùng tỉ lệ đầu/cơ thể, đường chân và trang phục qua cả hàng; không bake bóng. |

### AN04 — animation — grid 4 × 3

Canvas kế hoạch: 2048 × 1536 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `guard_right_walk_0` | M12 | Bảo vệ tuần tra ngang, tay gắn chìa khóa/đèn pin rời; bước 0, cùng feet line, cầm đúng anchor qua cycle. |
| 2 | `guard_right_walk_1` | M12 | Bảo vệ tuần tra ngang, tay gắn chìa khóa/đèn pin rời; bước 1, cùng feet line, cầm đúng anchor qua cycle. |
| 3 | `guard_right_walk_2` | M12 | Bảo vệ tuần tra ngang, tay gắn chìa khóa/đèn pin rời; bước 2, cùng feet line, cầm đúng anchor qua cycle. |
| 4 | `guard_right_walk_3` | M12 | Bảo vệ tuần tra ngang, tay gắn chìa khóa/đèn pin rời; bước 3, cùng feet line, cầm đúng anchor qua cycle. |
| 5 | `luon_box_right_walk_0` | M29 | Lươn đi ngang hai tay đỡ thùng rời; bước 0, cùng feet line, cầm đúng anchor qua cycle. |
| 6 | `luon_box_right_walk_1` | M29 | Lươn đi ngang hai tay đỡ thùng rời; bước 1, cùng feet line, cầm đúng anchor qua cycle. |
| 7 | `luon_box_right_walk_2` | M29 | Lươn đi ngang hai tay đỡ thùng rời; bước 2, cùng feet line, cầm đúng anchor qua cycle. |
| 8 | `luon_box_right_walk_3` | M29 | Lươn đi ngang hai tay đỡ thùng rời; bước 3, cùng feet line, cầm đúng anchor qua cycle. |
| 9 | `luon_pot_right_walk_0` | M12 | Lươn đi ngang đỡ nồi rời; bước 0, cùng feet line, cầm đúng anchor qua cycle. |
| 10 | `luon_pot_right_walk_1` | M12 | Lươn đi ngang đỡ nồi rời; bước 1, cùng feet line, cầm đúng anchor qua cycle. |
| 11 | `luon_pot_right_walk_2` | M12 | Lươn đi ngang đỡ nồi rời; bước 2, cùng feet line, cầm đúng anchor qua cycle. |
| 12 | `luon_pot_right_walk_3` | M12 | Lươn đi ngang đỡ nồi rời; bước 3, cùng feet line, cầm đúng anchor qua cycle. |

### AN05 — animation — grid 4 × 3

Canvas kế hoạch: 2048 × 1536 px; trạng thái: `generated_sliced`.

| Ô | Asset/frame ID | Mission | Mô tả |
|---:|---|---|---|
| 1 | `long_leave_right_walk_0` | M29 | long phục trang long_office, bước 0 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 2 | `long_leave_right_walk_1` | M29 | long phục trang long_office, bước 1 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 3 | `long_leave_right_walk_2` | M29 | long phục trang long_office, bước 2 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 4 | `long_leave_right_walk_3` | M29 | long phục trang long_office, bước 3 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 5 | `to_leave_right_walk_0` | M30 | to phục trang to_office, bước 0 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 6 | `to_leave_right_walk_1` | M30 | to phục trang to_office, bước 1 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 7 | `to_leave_right_walk_2` | M30 | to phục trang to_office, bước 2 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 8 | `to_leave_right_walk_3` | M30 | to phục trang to_office, bước 3 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 9 | `dam_leave_right_walk_0` | M31 | dam phục trang dam_travel, bước 0 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 10 | `dam_leave_right_walk_1` | M31 | dam phục trang dam_travel, bước 1 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 11 | `dam_leave_right_walk_2` | M31 | dam phục trang dam_travel, bước 2 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |
| 12 | `dam_leave_right_walk_3` | M31 | dam phục trang dam_travel, bước 3 ngang tới cửa; vali gắn riêng, không đổi tay xuyên cycle. |

## Asset hiện có phải tái sử dụng

| ID | Công dụng |
|---|---|
| `R_LUON` | Lươn thường phục idle; walk_0–3 cùng hướng, flip trái/phải |
| `R_WALK` | Chu kỳ ngang hiện có gồm walk_0–3 |
| `R_BACKPACK` | Idle + backpack_walk_0–2; một hướng có flip |
| `R_BED` | Bộ enter/idle/sleep/exit hiện có |
| `R_STUDY` | Study_0–2 và notebook; desk lower/foreground native crop |
| `R_TYPE` | Typing_0–1 và sprite ngồi/laptop; không dùng cho sơ mi nếu lộ sai áo |
| `R_FRIENDS` | dam/long/to standing và sitting; nhận diện canonical |
| `R_ROOM` | Phòng nền trống + furniture lớp rời từ scene |
| `R_DESK` | Bàn/ghế và foreground native crop |
| `R_BUNK` | Giường tầng có sẵn, không đổi perspective bedding |
| `R_SWITCH` | Công tắc/ổ điện, animation ánh sáng bằng Unity |
| `R_HALL` | Hành lang; camera/foreground có sẵn |
| `R_WC` | WC Nam/Nữ; cửa/water/progress đã có prototype |
| `R_CAMPUS` | Đường đi bộ/trạm; chỉnh camera thay vì tạo lại |
| `R_BUS` | Xe buýt33 và bus_sign; hai chuyến khác ngày dùng cùng asset |
| `R_H6` | H6 gate_v2 + classroom_v2, không dùng revision cũ |
| `R_TEACHER` | Giảng viên generic; tái dùng visual cho GVHD/giáo sư lab với identity data riêng |
| `R_POT` | Pot_0/pot_1, steam; cùng một lẩu, không tạo lại |
| `R_FLOOR` | luon/dam/long/to_floor: pose ăn sàn, M08/M34 |
| `R_LAPTOP` | Laptop nguyên vẹn; screen là overlay |
| `R_BAGPROP` | Balo prop rách, thu dọn M44 |
| `R_TRASH` | trash_noodles/cans/bags/small và piles; bật/tắt theo năm |
| `R_CLEAN` | Chổi/hót rác; bucket/rag là mới |
| `R_DIPLOMA` | Bằng hiện có, không generate lại; sửa nhãn bằng runtime nếu cần |
| `R_DIALOGUE` | Dialogue box, portrait_luon/dam/long/to, button_yes/no |
| `R_GRADES` | Stamp F và A đã có; không generate lại |
| `R_SHADOW` | Shadow character/bed/desk/small tách khỏi sprite |

## UI và hệ thống dựng bằng Unity

| ID | Công dụng |
|---|---|
| `U_HUD` | HUD ví/năng lượng/bromance/tín chỉ/giờ/ngày/kỳ; objective và prompt bàn phím/touch |
| `U_DIALOGUE` | Speaker/portrait/thoại/lựa chọn/next; crop portrait NPC từ sprite, font Việt |
| `U_INVENTORY` | Inventory, kéo thả giấy, nhận đồ, item attach/hand anchor; bàn giao chìa khóa |
| `U_CLEAN` | Bốn điểm bụi bằng mask/tint, timer60s, progress, retry không mua đồ lại |
| `U_MEAL` | Chọn món/giá/budget; slots khay cơm; xác nhận bữa ăn |
| `U_TRAVEL` | Zone/marker/camera/route, vé7k, điểm đón phụ/chuyến sau; trip/day reset |
| `U_CLASS` | Puzzle warm-up1…10=55; lecture, hint20s/retry; cùng lớp là NPC mới |
| `U_ACADEMIC` | Portal, course graph/điểm/tín chỉ/cảnh cáo/đăng ký/điều kiện đồ án; academic card |
| `U_PHONE` | Điện thoại: nhóm404, khách/crush, gọi mẹ, job, lịch thi/lễ và số dư |
| `U_DESKTOP` | Window giả lập chung: text/code/browser/forms/buttons/scroll; không ảnh OS tĩnh |
| `U_TIMELAPSE` | Clock/light/room state qua ngày/kỳ; montage và sổ thu chi |
| `U_GAMING` | YES/NO có nhánh hội tụ; màn game giả lập và âm loop |
| `U_WAKE` | Dậy/Snooze, alarm display, blur, flags điểm danh; hỗ trợ touch |
| `U_STEALTH` | 10s, phích/nồi/tủ/chăn, vùng phát hiện, fail/retry; overlay cone đèn |
| `U_CODE` | Editor/log/patch/test/submit; M13, M39, M42 chung framework |
| `U_EXAM` | Đề/đáp án/feedback/tiến độ theo kiến thức; retry/assist; không ép20% dù trả lời đúng |
| `U_INSTALL` | Boot/backup/chọn ổ/confirm/installer/Office giả lập/progress/reward |
| `U_JOBS` | Nhận/bàn giao job, cứu dữ liệu/vệ sinh/thiết lập mạng có đồng ý; ledger tiền và phí |
| `U_REPAIR` | Scan/quarantine/diagnostic/nguồn; checklist sửa máy |
| `U_CV_WORD` | Chỉnh font/lề/giãn dòng/mục lục/export/in; dùng chung CV và báo cáo |
| `U_PHOTO` | Viewfinder/flash/capture render scene; album có texture sinh runtime |
| `U_ROOM_STATE` | Room prefab nhiều trạng thái; NPC/đồ giường/biển/rác; góc Lươn dọn trống |
| `U_UNINSTALL` | Icon vector/menu/hold đúng nút/cancel/complete; touch giữ tương đương |
| `U_RESEARCH` | Tìm tài liệu, block graph, nối cổng, validate và nhật ký đề tài |
| `U_DEMO` | Demo thiết bị mạng, feedback bảo mật, kết quả test và task sửa |
| `U_DEFENSE` | Slide UI/chuyển slide/confidence/Q&A/score/retry, 5 người hội đồng |
| `U_ENDING` | Thu dọn, tắt đèn, trả khóa, nhận bằng, cụng ly/khoác vai, fade/credits/menu |
| `U_COMMON` | Menu/pause/settings/save/load/assist/audio; khóa input một chủ, load stable checkpoint |
| `U_VFX` | Steam/water/wind/glow/flash/fade/confetti bằng shader/particles/tween |

## Flow và phần ngoài imagegen

Áp dụng [PRODUCTION_FLOW_BASELINE.md](PRODUCTION_FLOW_BASELINE.md) cho 45 entry/action/exit và [FULL_FLOW_AUDIT_BEFORE_GENERATION.md](FULL_FLOW_AUDIT_BEFORE_GENERATION.md) cho lỗi và nghiệm thu. Dữ liệu, audio, các phát sinh có điều kiện và checkpoint nằm tại mục 8–11 của [REMAINING_ASSET_INVENTORY.md](REMAINING_ASSET_INVENTORY.md).

Các điểm bắt buộc: M01–05 đầy đủ; M07 có chuyến xe dự phòng; NO/snooze có hậu quả rồi hội tụ; học phần đúng tiên quyết; ba người mới thay ba bạn; Đạm bay sau M34; bảo vệ đạt mới mở M44; chỉ dọn góc Lươn, trả khóa, nhận bằng/hội ngộ/credits rồi Finished.

## Cổng nghiệm thu còn lại

1. Duyệt chu kỳ đi bộ, feet/pivot, outfit, contact pose và carry prop trong chuyển động.
2. Duyệt camera, ghế/bàn/foreground, wardrobe và room state trong scene thật.
3. Xác nhận import/catalog theo hash hiện tại; reference đúng trong gameplay.
4. Hoàn thiện UI/data/audio; kiểm tra đủ 45 mission và các nhánh retry/cancel/thiếu tiền.
5. Save/load tại transition; không nhân đôi tiền/đồ/tín chỉ; keyboard/touch/pause hoạt động nhất quán.
6. Chỉ đánh hoàn tất sau full playthrough và kiểm tra ending/mobile/build.
