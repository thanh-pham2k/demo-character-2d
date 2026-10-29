# Audit và kế hoạch — Vietnamese University Life Simulator

Trạng thái: bản làm việc, chưa hoàn tất. Lưu ngoài repo để giữ nguyên trạng thái repo vừa dọn theo yêu cầu. Nguyên liệu: ba tệp yêu cầu trong `/Users/thanhpv/.codex/attachments/6d24400b-b9fb-4579-bb63-adfbacaa60f1/`. Nội dung dưới đây là thiết kế đề xuất, không phải gameplay đã triển khai.

## Phạm vi và ràng buộc

Từ nhập học đến tốt nghiệp năm 4–6, tìm việc và ngày đầu đi làm; năm 5–6 là nhánh có nội dung riêng. Game life-sim Việt Nam, lựa chọn làm thay đổi thời gian, tình trạng học tập, quan hệ, cơ hội, ký ức và kết quả nghề nghiệp. Không sao chép nội dung hoặc cơ chế cụ thể của game tham khảo. Mọi con số kinh tế và tỷ lệ dưới đây phải qua thử nghiệm, không coi là cân bằng đã xác nhận.

## Audit hiện trạng

- Repo đã được người dùng dọn và push: không có gameplay hoặc bộ tài liệu đang hoạt động; các thiết kế từng xóa không được mặc định phục hồi. Ba tài liệu người dùng cung cấp là đầu vào thiết kế, không phải hệ thống đã triển khai.
- Input 1 mô tả fantasy, arc và năm; input 2 nêu lỗ hổng về hệ thống, event graph, content density, economy và ending; input 3 yêu cầu tài liệu có thể triển khai, tiến hành 20 phase và kiểm tra toàn bộ DoD.
- Các ví dụ trong input như số lượng playable days, điểm số, nghề và sự kiện là mục tiêu hoặc giả thuyết; chưa đủ chứng minh khả thi. Không gán nhãn production-ready trước khi có nội dung, số liệu và kiểm tra tương ứng.
- Nhu cầu đã nêu: 5–6 NPC chính; 8–12 NPC phụ; mục tiêu tham khảo 60–80 major, 120–180 medium, 200–300 small; 6–8 hướng nghề, 12–15 địa điểm, 40–60 hoạt động, 20–30 môn, 60–100 topic. Cần ma trận thực tế, ngân sách sản xuất và chứng minh độ phủ trước khi tuyên bố đạt.
- Ranh giới video YouTube: nguồn được cung cấp nhưng công cụ web chưa truy xuất được nội dung. Không khẳng định đã xem video hoặc benchmark chi tiết từ video. Trang chính thức Growing Up mô tả quản lý lịch, kỹ năng, trường học, tình bạn, hoạt động và các hướng tương lai: https://store.steampowered.com/app/1191120/ . Trang danh mục simulation chỉ là điểm khám phá, không phải bằng chứng cho gameplay cụ thể.

## Benchmark có thể kiểm chứng

| Trục | Bằng chứng công khai | Lý do hấp dẫn | Rủi ro | Hướng thiết kế đề xuất |
|---|---|---|---|---|
| Lịch và lựa chọn | Trang Growing Up mô tả quản lý lịch và hoạt động | Chọn việc khiến thời gian có giá trị | Dồn tối ưu làm mất cảm giác sống | Cho thấy chi phí và cơ hội bị bỏ lỡ, cho phép đổi kế hoạch khi có tin mới |
| Kỹ năng và tương lai | Trang Growing Up mô tả học kỹ năng và tương lai nghề | Hoạt động nhỏ nối với mục tiêu dài | Cây kỹ năng trở thành bảng điểm | Course/topic nối project, portfolio và cơ hội, nhiều đường vào cùng nghề |
| Tình bạn và quan hệ | Trang Growing Up mô tả kết bạn và quan hệ | Người chơi muốn biết nhân vật sẽ phát triển thế nào | Chỉ tăng điểm, NPC chờ vô hạn | Trạng thái tin cậy, mâu thuẫn, lịch riêng và callback có điều kiện |
| Giai đoạn cuộc đời | Trang mô tả từ nhỏ đến trưởng thành | Thế giới đổi theo thời gian | Nội dung sau chỉ đổi chỉ số | Mỗi năm đổi cơ hội, môi trường và quan hệ; năm 5–6 là recovery có nội dung riêng |

Các đánh giá “lý do”, “rủi ro”, “hướng thiết kế” là suy luận/đề xuất, không phải claim về hoạt động cụ thể của Growing Up. Chưa có dữ liệu đủ để so sánh định lượng content density, gating, economy hoặc ending; cần nghiên cứu bổ sung trước khi kết luận.

## Thứ tự và trạng thái phase

1. Audit: **đang làm**; bổ sung benchmark trang chính thức tại `42_Benchmark_nguon.md`; vẫn thiếu truy cập video, quan sát playthrough và bằng chứng định lượng.
2. Pillars + core loop: **bản nháp** trong `01_Nen_tang.md`; chưa đánh giá bằng playtest.
3. Global systems: bản nháp thời gian/event và học tập/quan hệ trong `02_He_thong_thoi_gian_va_su_kien.md` và `03_Hoc_tap_va_quan_he.md`; đã thêm energy, money và career trong `04_Tai_nguyen_va_nghe_nghiep.md`; có khung 8 hướng nghề tại `32_Huong_nghe_nghiep.md`; còn thiếu records đầy đủ, progression graph có thể chạy và mô phỏng.
4. Kiến trúc 6 năm và 5. graph mở khóa: đã có bản nháp trong `05_Kien_truc_sau_nam.md` và `06_Progression_va_gate.md`, chưa đủ dữ liệu kiểm chứng; 6. NPC bible: hồ sơ nháp và scene seeds trong `07_NPC_bible.md`, chưa có event production-ready; 7. location/activity: catalogue thử nghiệm trong `08_Dia_diem_va_hoat_dong.md`, chưa có data runtime hoặc cân bằng; 8. Year 1 matrix: `09_Nam_1_matrix.md` bản nháp; 9. Year 1 events: chuỗi buổi sáng, học/roommate, nhận phòng và cuối năm trong `10_Nam_1_su_kien_mau.md`, `12_Nam_1_hoc_va_ban_cung_phong.md`, `13_Nam_1_den_va_ket.md`; còn thiếu nhiều records và dữ liệu calendar; 10. rà soát logic thủ công một fixture trong `11_Nam_1_ra_soat_logic.md`; simulation và playtest toàn Năm 1 chưa làm; 11. Năm 2: matrix và event mẫu trong `15_Nam_2_matrix.md`, `16_Nam_2_su_kien_mau.md`; các event khác và simulation còn thiếu; 12. Năm 3: matrix và event mẫu trong `17_Nam_3_matrix.md`, `18_Nam_3_su_kien_mau.md`; nhiều event và simulation còn thiếu; 13. Năm 4: matrix và event mẫu trong `19_Nam_4_matrix.md`, `20_Nam_4_su_kien_mau.md`; event đầy đủ và simulation còn thiếu; 14. Năm 5: matrix và event mẫu trong `21_Nam_5_matrix.md`, `22_Nam_5_su_kien_mau.md`; events và simulation chưa đủ; 15. Năm 6: matrix và event mẫu trong `23_Nam_6_matrix.md`, `24_Nam_6_su_kien_mau.md`; events và simulation chưa đủ; 16. graduation: đặc tả nháp `25_Tot_nghiep_va_ban_giao.md`, chưa có program data/simulation; 17. job search: `26_Tim_viec_sau_tot_nghiep.md` bản nháp, thiếu rubric/cơ hội thay thế; 18. ending: đặc tả nháp `27_Ending_va_memory.md`, thiếu fragment data và simulation; 19. balance/replay; 20. consistency audit: **chưa làm**.

## Gate chưa đạt

Không có data catalog, event records, NPC arcs đầy đủ, runnable simulation, chứng cứ playtest hoặc tính điểm có cơ sở. Chưa chấm 9/10 và chưa coi DoD hoàn tất. Các phần tiếp theo phải liên kết ID, điều kiện, tác động, recovery và callback; kiểm tra cả nhánh không chọn.
