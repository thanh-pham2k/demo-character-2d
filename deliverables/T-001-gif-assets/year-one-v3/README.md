# Storyboard Năm 1 — bản nháp đang rà

Mười sheet 4×3 được tạo bằng imagegen tích hợp, dùng reference phòng 404, Luồn và Long trong `mobile-2d/Assets/Sprites/MVP`. Mỗi sheet được cắt theo 4 cột × 3 hàng; mã `bNN-XX` chỉ ô XX trong batch NN theo thứ tự trái sang phải, trên xuống dưới. Các bản cắt trùng với `ordered/` đã được dọn; `b01-03.png` là reference bố cục được chọn, không phải asset nền Unity đã tách. `batch-01`/`04` kích thước 1448×1086; các batch còn lại 1536×1024. Tọa độ crop mặc định làm tròn `cột × chiều rộng / 4`, `hàng × chiều cao / 3`; batch 02 chia không đều nên crop theo x=[0,434,854,1197,1536], y=[0,340,671,1024], trừ lề 4 px. Batch 09–10 trừ lề 4 px mỗi ô. Pivot/PPU không áp dụng cho storyboard.

- Batch 01: nhận phòng, chọn giường, căn tin, buổi sáng và lời rủ của Long.
- Batch 02: hai laptop, đi học, ôn và thi HK1. Ô 05 và 09 vẽ nhầm Long tại dashboard; xem batch 05 ô 01 và 02 thay thế sơ bộ.
- Batch 03: kết quả HK1, nhánh bus, lớp học, phòng ban đêm và chuẩn bị HK2.
- Batch 04: thi cuối năm, về phòng, chuẩn bị và ăn lẩu, nghỉ cuối năm.
- Batch 05: các cảnh phòng bổ sung nhằm thống nhất vị trí đồ đạc và sửa nhân vật tại dashboard.
- Batch 06: chọn giường, hai laptop, đèn và lẩu; dùng thêm sprite Đạm và Tộ làm reference. Batch 07: đặt balo, thức dậy, đánh răng, rửa mặt, chuẩn bị sách, rời game và về giường.
- Batch 08: dọn phòng, căn tin, chuyến xe sau, trễ lớp và kết năm; ảnh 01–12 đọc từ trái sang phải, trên xuống dưới. Tạo theo prompt 12 cảnh độc lập với reference batch 06 cùng sprite Luồn/Long, giữ phòng và tách thời điểm; hình không có chữ hoặc UI.
- Batch 09: sửa continuity giường dưới, dọn phòng, hai laptop, nhánh đèn và kết năm. Batch 10: 12 cảnh nhánh giường trên từ lựa chọn tới ngủ sau lẩu. Reference batch 06 và sprite Luồn/Long; yêu cầu 4×3, chia ô đen, không chữ. Các mô tả này tóm tắt prompt; prompt đầy đủ của các lượt tạo được lưu trong lịch sử chat, không được tái dựng thành nguyên văn tại đây.

`ordered/INDEX.md` ánh xạ 40 ID trong spec sang ảnh đại diện; `ordered/bo-sung` chứa các thao tác bổ sung theo thứ tự batch 07. `year-one-ordered-draft.zip` đóng gói danh mục và ảnh tương ứng. Cảnh đại diện không bao hàm mọi bước hoặc giao diện được mô tả trong một ID.

Bộ theo thứ tự dùng ảnh được chọn và các cảnh bổ sung từ nhiều batch. Một số ảnh đại diện được tái sử dụng; dashboard là hình minh họa, chữ và trạng thái phải thêm khi dựng GIF. Khách ở batch 04 chưa bám reference Đạm/Tộ; dùng cảnh lẩu batch 06. Những ảnh từ các batch khác nhau có thể lệch bố cục và không phải animation liên tục. Không tích hợp các ảnh này vào Unity.
