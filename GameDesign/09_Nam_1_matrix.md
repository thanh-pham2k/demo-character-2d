# Năm 1 — content matrix v0.1

**Trạng thái:** ma trận authoring, chưa phải lịch runtime hay bộ event hoàn chỉnh. Chủ đề: thích nghi, tự lập và hình thành tình bạn; câu hỏi: “Tôi sẽ sống đời sinh viên thế nào?”. Các tuần dưới đây là thứ tự thiết kế (W01–W12), không khẳng định là toàn bộ 52 tuần hoặc ngày skip. Ngày/giờ, course ID, credit, duration, NPC availability và ràng buộc thực tế phải chốt trong calendar trước khi triển khai.

## Ma trận

| Tuần / event seed | Học tập | Long | Minh / Hạnh | NPC khác | Career | Địa điểm/cơ hội mới | Lựa chọn chính | Hậu quả | Callback dự kiến |
|---|---|---|---|---|---|---|---|---|---|
| W01 `Y1_ARRIVAL` | Biết lịch nhập học | L01 gặp; L02 mời ăn | Chưa gặp | S01 nhận phòng | Chưa mở | LOBBY, ROOM, CORRIDOR | Đi ăn cùng / dọn / đi riêng | Thời gian; memory bữa ăn nếu tham gia | L05 chỉ nhắc việc Long biết |
| W01 `Y1_FIRST_NIGHT` | Biết giờ học đầu | L05 trò chuyện tùy rảnh | — | S01 thông báo giờ KTX | — | WC, STAIRS, CANTEEN | Trò chuyện / gọi nhà / nghỉ | Lịch sáng và energy theo activity | Memory đêm đầu nếu event diễn ra |
| W02 `Y1_FIRST_CLASS` | Buổi học đầu, điểm danh | Có thể đi riêng theo lịch | M01, H01 nếu có mặt | S03 theo tuyến | — | BUS, H6, CLASS | Chuẩn bị hay rời phòng sớm | Đúng giờ / muộn / vắng theo timestamp | H02 nếu Hạnh chứng kiến trễ |
| W02 `Y1_LONG_GAME` | Lịch học đã biết | L03 rủ chơi trước lớp | Lớp tiếp nhận theo giờ | S03 | — | LIBRARY khi biết | Đi học / chơi một lượt / chơi tiếp | Thời gian, memory, nguy cơ lỡ xe | L04 chỉ khi Long biết kết quả |
| W02 `Y1_EVENING_CHOICE` | Chọn chủ đề cần học | Long có lời mời riêng | M02 nếu đã gặp và rảnh | — | — | ROOM, LIBRARY | Học / chơi / nghỉ | Evidence hoặc memory; thời gian trôi | W04 ôn theo evidence thực |
| W03 `Y1_ASSIGNMENT` | Nhận bài và deadline | L07 có thể nhờ giúp | M03 trao đổi khi cùng topic | S04 có thể xuất hiện | — | CLASS, LIBRARY | Làm bài / học topic / hoạt động khác | Bài dở hoặc nộp, lỡ deadline có recovery author | H04 chỉ khi bài trễ |
| W04 `Y1_MIDTERM_PREP` | Xem topic yếu, chọn ôn | L06 bàn về ôn thi | Minh có lịch học riêng | — | — | ROOM, LIBRARY | Ưu tiên topic nào, tự học hay nhóm | Evidence thay đổi; không hoàn thành mọi thứ là hợp lệ | Feedback kì giữa kỳ |
| W05 `Y1_MIDTERM` | Thi và feedback strong/neutral/weak | Phản ứng tùy kết quả được chia sẻ | Hạnh/Minh theo phân lớp | — | — | CLASS | Vào thi / bỏ lỡ / nộp sớm | Topic feedback, không tự cấp credit | W06 review chủ đề yếu |
| W06 `Y1_RECOVERY` | Học bù hoặc sửa bài nếu course cho phép | Có thể mời nghỉ | Office hour nếu Hạnh rảnh | S04 | — | LIBRARY | Bù topic / bài / nghỉ | Đổi thời gian lấy cơ hội cải thiện | Ghi evidence hoặc overdue |
| W07 `Y1_ROOM_CONFLICT` | Deadline cạnh tranh giờ nghỉ | L09 cần ngủ | — | Phòng bên nếu event có thật | — | ROOM | Đèn bàn / thỏa thuận / không đổi | Conflict state và thời gian; không mặc định hòa giải | L10/L11 theo outcome |
| W08 `Y1_GROUP` | Buổi học nhóm hoặc cá nhân | Có lịch riêng | M03 học cùng nếu rảnh | S04 học chung | Project seed, chưa mở nghề | LIBRARY | Học nhóm / tự học / nghỉ | Evidence, memory nếu cùng học | Lời mời nhóm Y2 chỉ khi có eligibility |
| W09 `Y1_FINAL_PREP` | Xem attendance, assignment, eligibility và topic | L13 tùy quan hệ/lịch | Hạnh giải thích rule được công bố | — | — | ROOM, LIBRARY | Làm bài / ôn / học bù | Có thể còn thiếu; UI nêu recovery | Resolver thi dùng state thật |
| W10 `Y1_FINAL` | Mỗi môn có exam/window riêng | — | Giảng viên tùy lịch | — | — | CLASS | Vào thi / đến muộn / bỏ thi | Pass/fail theo course definition; credit một lần | Tổng kết, lịch học lại nếu fail |
| W11 `Y1_HOTPOT` | Xem kết quả hoặc chờ kết quả | L14 tùy availability | Minh chỉ khi gặp và được mời | Khách khác theo event | — | ROOM | Chuẩn bị cùng / tham gia / từ chối | Memory và relation theo lựa chọn | L15 chỉ dùng những sự kiện đã xảy ra |
| W12 `Y1_RESOLVER` | Chốt course records, thiếu hụt | Quan hệ/conflict giữ nguyên | Dữ liệu biết của NPC giữ nguyên | — | Evidence hiện có | Calendar Y2 | Xem hậu quả skip → hủy/xác nhận | Snapshot, không tự cấp credit | Y2 dùng state đã commit |

## Đặc tả khung năm theo checklist 22 mục

1–2. Theme/câu hỏi: như trên. 3. Calendar: W01–W12 là seed, chưa có timestamp. 4. Academic: course, assignments, midterm, final theo schema; chưa có danh mục course/weights cụ thể. 5. Hệ mới: thời gian, học tập, tiền/năng lượng, quan hệ; chưa có runtime. 6. Location: 404, KTX, WC, sảnh, căn tin, bus, H6, lớp, thư viện; route cụ thể chưa author. 7. Activity: danh mục trong `08_Dia_diem_va_hoat_dong.md`.

8. NPC chính: Long, Minh, Hạnh; An/Nam không buộc xuất hiện năm 1. 9. NPC phụ: S01, S03, S04; ca/role cần author. 10–12. Major, medium, small: seed trong matrix, chưa phân loại và chưa đủ record. 13. Academic milestones: first class, assignment, midterm, final, review. 14. Relationship: gặp, lời mời, mâu thuẫn, callback; optional. 15. Career: kiến thức/evidence ban đầu, không gắn nghề cứng.

16–17. Failure/recovery: lỡ xe → chuyến có lịch tiếp theo; bài trễ → chỉ có recovery nếu định nghĩa assignment; thi kém → cơ hội ôn và retake theo course. Nguồn/điều kiện và thời điểm từng recovery còn thiếu. 18–19. Unlock/lock: phụ thuộc entry/địa điểm/NPC và thời gian; invitation hết hạn không khóa toàn campaign. 20. Resolver: tính riêng từng course và commit snapshot; logic assessment cần course data. 21. Memory: chỉ ghi event đã commit và actor biết sự kiện. 22. Carry-over: `YearSnapshot` trong `05_Kien_truc_sau_nam.md`.

## Các lỗ hổng phải giải trước production

Chưa có course IDs, topic prerequisites, lịch lớp/bus, timeslot, event database, số lượng điểm, giá, quy tắc eligibility và lịch retake. Một số ID trong matrix là event seed chưa liên kết event record; chưa có dữ liệu cho 60–80 major của toàn game. Chưa chứng minh có thể đi kịp xe, đủ thời gian ôn, có đường vào năm 2 hay giữ nguyên state khi skip/save lỗi. Chưa mô phỏng hoặc playtest. Phải phân biệt Long biết “đã chơi cùng” với Long biết player đi muộn; không tự truyền thông tin giữa NPC.
