# Game Design Bible — mục lục và trạng thái

**Trạng thái: đang xây dựng; chưa hoàn tất.** Tài liệu này nằm ngoài repo theo yêu cầu giữ repo sạch. Nội dung là đề xuất cho life-sim đại học Việt Nam, không phải gameplay, asset hay dữ liệu đã được triển khai. Fixture và giá trị là giả lập. Chưa có simulation toàn campaign, review chuyên môn hoặc playtest; không có cơ sở tuyên bố đạt 9/10.

[Bàn giao và trạng thái audit](67_Trang_thai_ban_giao.md) · [Đề xuất demo core MVP](66_Demo_core_MVP.md)

## Đọc từ đâu

1. [Audit và kế hoạch](00_Audit_va_ke_hoach.md) — phạm vi, các phase và giới hạn nguồn.
2. [Nền tảng](01_Nen_tang.md) — fantasy, vòng chơi, một lượt quyết định.
3. [Kiến trúc sáu năm](05_Kien_truc_sau_nam.md) và [progression](06_Progression_va_gate.md) — hướng xuyên năm và gate.
4. [Benchmark](42_Benchmark_nguon.md) — phân biệt nguồn Growing Up với đề xuất; video YouTube chưa truy cập được.

## Hệ thống

- [Thời gian và sự kiện](02_He_thong_thoi_gian_va_su_kien.md), [boundary/reservation](29_Boundary_va_reservation.md), [schema event](54_Event_schema.md).
- [Học tập và quan hệ](03_Hoc_tap_va_quan_he.md), [tài nguyên và nghề](04_Tai_nguyen_va_nghe_nghiep.md), [timer và tiếp cận](40_Policy_timer_va_accessibility.md), [save/transaction](43_Save_state_va_transactions.md).
- [Chương trình học](31_Chuong_trinh_hoc.md), [hướng nghề](32_Huong_nghe_nghiep.md), [skill graph](35_Skill_graph_fixture.md), [project](36_Project_career_fixture.md).
- [NPC Bible](07_NPC_bible.md), [địa điểm và hoạt động](08_Dia_diem_va_hoat_dong.md), [sự kiện NPC phụ phần 1](45_NPC_phu_su_kien.md), [phần 2](46_NPC_phu_tiep.md).

## Nội dung theo năm

| Năm | Matrix | Event mẫu | Dữ liệu học tập |
|---|---|---|---|
| 1 | [Matrix](09_Nam_1_matrix.md) | [Buổi sáng](10_Nam_1_su_kien_mau.md), [học/Long](12_Nam_1_hoc_va_ban_cung_phong.md), [đến và kết](13_Nam_1_den_va_ket.md), [hội thoại Long](44_Long_hoi_thoai_nam_1.md), [lớp đầu](48_Buoi_hoc_dau.md), [đi học D8](49_D8_phuong_an_di_hoc.md) | [NMT](14_Fixture_mon_va_de_thi.md), [thi lại](30_NMT_thi_lai.md), [5 môn nền](50_Cac_mon_nen_nam_1.md), [calendar mẫu](34_Lich_nam_1_fixture.md), [Long nhờ giúp và đổi kế hoạch](58_Long_giup_replan.md), [bữa lẩu](59_Hotpot_nam_1.md), [học nhóm với Minh](60_Hoc_nhom_nam_1.md), [gặp Minh](61_Gap_Minh_nam_1.md), [chuẩn bị thi cuối](62_Chuan_bi_thi_cuoi.md), [đường tới thi](63_Duong_di_thi_cuoi.md), [chuyển Năm 2](64_Chuyen_nam_1_sang_2.md), [mở đầu Năm 2](65_Mo_dau_nam_2.md) |
| 2 | [Matrix](15_Nam_2_matrix.md) | [CLB, việc, nhóm](16_Nam_2_su_kien_mau.md) | [Môn Y2](51_Cac_mon_nam_2.md) |
| 3 | [Matrix](17_Nam_3_matrix.md) | [Project, hackathon, CV](18_Nam_3_su_kien_mau.md) | [Môn Y3](52_Cac_mon_nam_3.md) |
| 4 | [Matrix](19_Nam_4_matrix.md) | [Thực tập, đồ án](20_Nam_4_su_kien_mau.md) | [4 môn](53_Cac_mon_nam_4.md), [thực tập](37_Hoc_phan_thuc_tap.md), [đồ án](38_Do_an_va_bao_ve.md) |
| 5 | [Matrix](21_Nam_5_matrix.md) | [Phục hồi](22_Nam_5_su_kien_mau.md) | Chưa có calendar toàn năm |
| 6 | [Matrix](23_Nam_6_matrix.md) | [Học lại, gặp bạn, chốt](24_Nam_6_su_kien_mau.md) | Chưa có calendar toàn năm |

## Tốt nghiệp, nghề và kiểm toán

[Tốt nghiệp](25_Tot_nghiep_va_ban_giao.md) · [Tìm việc](26_Tim_viec_sau_tot_nghiep.md) · [Cơ hội tìm việc fixture](33_Thi_truong_viec_lam_phuc_hoi.md) · [Ending/memory](27_Ending_va_memory.md).

[Kiểm toán nhất quán](28_Ra_soat_nhat_quan.md) · [đối chiếu ID](47_Doi_chieu_ID.md) · [rà logic Năm 1](11_Nam_1_ra_soat_logic.md) · [hai trace NMT](39_Kiem_toan_fixture.md) · [ngân sách thời gian/E](41_Kiem_toan_time_economy.md). Đây đều là rà soát thủ công, không phải test/simulation.

## Chưa đạt DoD

- Chưa có event database đủ độ phủ, lịch và roster/availability toàn campaign, narrative arcs 12–20 cảnh hoàn chỉnh cho mỗi NPC chính hoặc dữ liệu các opportunity thay thế.
- 24 course IDs và một số bài/câu hỏi mới là fixture; chưa đủ lessons, exam variants, retake, rubric/capstone, review chuyên môn và lịch để chứng minh graduation.
- Tám hướng nghề là seed; thiếu company/JD, internship, rubric, skill effects, nguồn sống và chính sách sau đợt tìm việc cuối.
- Các contract save, timer và event là đặc tả chưa có implementation hay bằng chứng chạy. Chưa có schema data canonical/validator; cần kiểm tra các ID và rule cũ khi chuyển dữ liệu.
- Chưa có evidence playtest, đo balance, coverage/branching, chạy replayability, kiểm toán end-to-end hoặc scorecard hợp lệ.

Các bảng matrix không phải event records production-ready. Khi có xung đột, không tùy ý chọn một file cũ làm canon: ghi issue và thống nhất definition có version trước khi code.
