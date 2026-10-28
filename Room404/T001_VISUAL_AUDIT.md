# Đối chiếu hình ảnh T-001 — bản web Room404

Nguồn yêu cầu: `tasks/active/T-001-year-one-gameplay-plan.md`. Trạng thái dưới đây dựa trên source và asset hiện có; không xác nhận gameplay, tỉ lệ hay animation đã được kiểm tra thủ công. Tracker `tasks/README.md` ghi rõ scene/gameplay Unity cũ đã reset ngày 27/10/2026; không dùng báo cáo runtime cũ làm bằng chứng hiện tại.

| Mốc | Cảnh/ảnh cần thiết | Asset hiện có | Khoảng trống ở web |
|---|---|---|---|
| Day 0 | Quầy, hành lang, 404, ba lô, dọn đồ, roommate, căn tin, ngủ | Phòng và hành lang/WC/sảnh trong `assets/`; quầy và căn tin concept trong `deliverables/core-year1/batch-01` và `batch-02` | Chưa có flow quầy/giấy/chìa, dọn đồ, roommate, căn tin; nhân vật phụ chưa được gắn và duyệt nhận diện |
| Day 1 | Routine WC, sách/ba lô, xe 33, H6, lớp, về phòng | Atlas WC/ba lô/cầu thang trong `assets/luon/`; concept trạm/H6/lớp trong batch-02; xe riêng tại `assets/journey/bus-v1` | Xe mới là asset draft; chưa có lên xe, lịch, H6/lớp hoặc về phòng |
| Day 2 | Nhân vật Long, học/chơi, lớp, nhắc giờ | `desk-bag-v1`, `actions-v2`; Long concept tại `deliverables/core-year1/batch-03` | Chưa có Long trong web, đối thoại/choice và cảnh lớp; atlas học/game web dùng chung tư thế |
| Day 28 | Dashboard ôn giữa kỳ, học topic, feedback | Mini-game bàn hiện có | Chưa có dashboard/topic hay biểu diễn feedback T-001 |
| Day 29 | WC, bus, H6, lớp, về phòng | Concept batch-02 | Chưa có các cảnh/flow nối với web |
| Day 30 | Phòng thi, làm bài, feedback theo topic | Concept lớp/thi batch-02 | Chưa có scene thi hoặc UI/feedback theo topic |
| Day 92 | Dashboard, bài tập, học bù và ôn | Mini-game bàn chung | Chưa có UI môn, deadline, recovery hoặc feedback tương ứng |
| Day 99 | Dashboard, lịch, phòng thi, câu hỏi/recap | Concept lớp batch-02 | Chưa có scene, UI thi và recap |
| Day 113 | Routine, bus đến/trễ, H6, lớp | Trạm concept và bus draft | Chưa có countdown, xe đến/rời, nhánh lỡ xe hoặc lớp |
| Day 155 | Học/game/Long, đèn, hai người nghỉ | Ánh sáng và atlas đèn; Long concept | Chưa có Long và cảnh tương tác chung |
| Day 204 | Dashboard HK2, lớp, bài tập/ôn/recovery | Concept lớp và mini-game bàn | Chưa có scene lớp, dashboard và UI tương ứng |
| Day 211 | Thi, recap, chuẩn bị và ăn lẩu cùng bạn | Lẩu concept `batch-06/08-hotpot.png`, bạn concept batch-03/04 | Nồi/bếp gộp, chưa có nguyên liệu tách lớp, nhân vật/bố cục/hoạt ảnh lẩu và recap |

## Ưu tiên visual kế tiếp

1. Duyệt/tách sprite xe buýt, cảnh lên/xuống và H6/lớp, bảo đảm cùng camera và tỉ lệ; không mặc định asset concept đã production-ready.
2. Chốt nhận diện Long và bạn cùng phòng theo reference, rồi tạo batch poses phục vụ trò chuyện, chơi, học, ăn lẩu.
3. Dựng batch đạo cụ tách lớp cho thủ tục, sách, bữa ăn và lẩu; tránh tạo lại đạo cụ có thể tái sử dụng.
4. Thiết kế UI và trạng thái hình cho dashboard, lịch, exam, recap, skip; những phần này phụ thuộc dữ liệu/gameplay chưa có trong web.
5. Kiểm tra thủ công bằng F8 sau khi từng flow được nối; tài liệu này không thay thế nghiệm thu.

Đã bổ sung batch đạo cụ draft `assets/t001-props-v1`: giấy tờ, chìa, sách, bếp, nồi, khay nguyên liệu, bát/đũa và khay cơm. Sách còn gộp thành chồng, nguyên liệu/bát còn gộp theo khay, chưa có các lớp tương tác riêng.
