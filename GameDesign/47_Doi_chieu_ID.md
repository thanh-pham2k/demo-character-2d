# Đối chiếu ID và liên kết — v0.1

Rà soát thủ công một số tham chiếu; không phải validator toàn bộ dữ liệu và chưa chứng minh event graph hoàn chỉnh.

## Đã sửa

1. Event chọn nhóm Năm 2 trước đây yêu cầu enrolled `C_PROJECT` (môn dự án cá nhân Năm 3, prerequisite C_TEAM); đổi thành `C_TEAM` tại `16`. Event này vẫn chưa có rubric/course calendar, không đồng nghĩa C_TEAM đã chạy.
2. LOC_CANTEEN tham chiếu `A_CANTEEN_SHIFT` nhưng catalogue định nghĩa `A_CAFE_SHIFT` dùng cho CAFE/CANTEEN; đã thống nhất `08` với ID có định nghĩa.
3. Callback `first_class` đã được dùng trong `22`/`45` nhưng chưa có nguồn sinh: `10` quy định tạo memory khi player thực sự dự entry `CLASS_NMT_FIRST`, không tạo khi vắng. Nội dung và actors cụ thể cần author, không tự thêm Minh/Hạnh vào memory.

## Vẫn thiếu

- Các event IDs trong matrix, `L01`–`L15`, `M01`–`M12` và các NPC khác phần lớn là seed, không nhất thiết khớp event definition. Cần catalog có ID canonical, type, source, prerequisites, outcome, callback và duplicate detection.
- Lịch có `CLASS_NMT_FIRST` nhưng event record `EV_Y1_ATTEND` hiện mô tả D2; fixture `48_Buoi_hoc_dau.md` đã mô tả attendance riêng; còn thiếu runtime và dữ liệu route/roster.
- `knownAt`/nguồn thông tin xe và availability actor chưa có records trong calendar. Thiếu event definition không được coi là UI có thể chạy.
- Các project, skill và JD có alias giữa `S_API`/`api_fundamentals`, `P_LIBRARY_API`/`project_library_api`; cần định nghĩa mapping trong schema, không tự thay chuỗi khi chưa có migration.
- Chưa có validator chạy trên canonical dataset; rà bằng `rg` chỉ cho thấy các tham chiếu được tìm, không chứng minh graph không thiếu/vòng.
