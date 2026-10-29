# Năm 1 — đến KTX và kết thúc năm v0.1

Đây là đặc tả ví dụ, chưa xác nhận runtime. Calendar, cost, credit và kết quả là dữ liệu giả lập để làm rõ outcome, không phải quy định thực tế. Các event áp dụng scheduler, save và idempotency trong `02_He_thong_thoi_gian_va_su_kien.md`.

## Fixture và điều kiện chung

Year 1 bắt đầu W01 14:00 tại LOC_LOBBY; player energy 75, wallet 30 M; `has_key=false`, `met_long=false`. Hai giường gác xép độc lập, mỗi giường có bàn phía dưới; không có giường tầng dưới. `bed_left`/`bed_right` trạng thái `free | assigned_player | assigned_long`. Long có thể tới sau nếu vắng lúc player mở cửa; không tự gặp hoặc tạo memory Long khi không có mặt. Lịch ở đây chỉ là ví dụ, không thay cho calendar campaign hoàn chỉnh.

## EV_Y1_RECEPTION — Đăng ký KTX

- Category major; W01 `[14:00,18:00)`, LOC_LOBBY, S01 available; trigger khi player tới quầy; required `has_key=false`, một instance.
- Player xem bảng thông tin (0 phút), xác nhận đăng ký (20 phút, E −2, không phí trong fixture) hoặc quay lại sau (0 phút). Khi hoàn thành, transaction cấp `key_404`, `has_key=true`, biết LOC_ROOM và lịch nhận phòng; không tự chuyển player.
- NPC đi hết ca hoặc player tới sau 18:00: thủ tục chưa hoàn tất, calendar có ca dự phòng W02 06:00–07:00 với cùng chi phí; không tự mở 404. Nếu activity bắt đầu trước 18:00 nhưng chưa hoàn tất thì ca hết làm activity gián đoạn; elapsed/E đã dùng được giữ và có thể tiếp tục ca dự phòng theo từng milestone đã lưu. Cần QA để chứng minh còn đủ thời gian trước buổi học đầu.
- Career/academic/relationship: không đổi. Memory `received_room_key` chỉ sinh khi commit. UI cho biết giờ ca và phí trước khi bắt đầu; save lỗi không cấp chìa hoặc trôi giờ nếu transaction thất bại.

## EV_Y1_ROOM_ENTRY — Cửa và chỗ ngủ

- Category major; sau nhận key và tới cửa 404, LOC_CORRIDOR; required `has_key=true`. Mở cửa mất 1 phút, E 0; sau hoàn tất chuyển LOC_ROOM, `room_entered=true`.
- Player chọn một trong hai giường gác khi nó còn free, hoặc để sau; không ép chọn giường của Long nếu giường đã assigned. Chọn mất 0 phút; sau khi commit `assigned_player` cho giường đó và giường kia được Long chọn khi Long thực sự có mặt. Khi chưa chọn, người chơi có thể xem phòng/ra ngoài; để ngủ phải chọn chỗ. Không tạo logic giường dưới.
- Nếu Long có mặt, `EV_Y1_MEET_LONG` được offer; nếu chưa, không kích hoạt dialog giả. Khi quay lại hoặc Long tới, cơ hội gặp mở lại trong cửa sổ đã author. Memory `first_404` sinh khi thực sự vào phòng; không chứa nội dung gặp Long trừ khi event gặp xảy ra.

## EV_Y1_MEET_LONG — Gặp bạn cùng phòng

- Category NPC; LOC_ROOM, `room_entered`, Long available, `met_long=false`; một instance. Mở lựa chọn chào hỏi hoặc để sau. `greet`: 5 phút E −1, đặt `met_long=true`, tạo memory `met_long`; trust +2. `later`: 0 phút, không tạo memory hay relationship; có thể gặp lại khi Long available.
- Player đã chọn giường: Long dùng giường free khi Long tới; player chưa chọn: Long chỉ đề xuất một giường, player được chọn giường còn lại hoặc thỏa thuận theo scene author. Không tự đổi giường player đã chọn. Long có thể mời ăn qua `EV_Y1_FIRST_MEAL` nếu thời gian phù hợp.
- Không có academic/career effect. Rời phòng hoặc hết lịch Long: event chưa resolve, cơ hội gặp lại theo lịch; không trừ trust vì không nói chuyện.

## EV_Y1_FIRST_MEAL — Lời mời ăn

- Category medium NPC, W01 `[17:00,20:00)`, đã gặp Long và Long available; 1 invitation. UI nêu đi cùng cần 10 phút travel đến căn tin, 45 phút ăn, 8 M, +5 E khi ăn hoàn tất; thời gian và tiền là fixture.
- `together`: xác nhận dự định đi cùng; player vẫn điều khiển travel. Khi cả hai có mặt và player chọn ăn, `A_MEAL_LONG` chạy; hoàn thành mới trừ 8 M, +5 E, tạo `first_meal_long`, trust +2. Nếu không đủ tiền, đưa lựa chọn bữa hỗ trợ `A_MEAL_SUPPORT` miễn phí 30 phút +5 E và cho Long ngồi cùng nếu available, không tự bắt Long trả tiền.
- `unpack`: 45 phút E −5, tạo `unpacked` khi hoàn tất; không tạo memory bữa ăn. `alone`: đi căn tin và chọn A_MEAL riêng, không có memory với Long. Cả hai lựa chọn không trừ trust tự động. Có thể bỏ lời mời khi tới trễ; không ghi bữa chung nếu chưa ăn.
- Career/academic không đổi; rủi ro là thời gian và tiền. L05 callback chỉ nhắc bữa ăn nếu `first_meal_long` có actor Long. Lưu trước/giữa ăn phải không trừ tiền hai lần. Giá đã nêu trước khi xác nhận; mất availability giữa travel có thể tiếp tục ăn một mình hoặc quay về.

## EV_Y1_FINAL_ELIGIBILITY — Trước kỳ thi

- Category scheduled; công bố lịch và lựa chọn chuẩn bị C_NMT D57 theo [62](62_Chuan_bi_thi_cuoi.md). Không đặt `eligible=true` khi thiếu rule hoặc dữ liệu.
- Chọn ôn topic, nghỉ nếu location cho phép, hoặc làm việc khác. ASG_01 đã đóng D22; hỏi giảng viên chỉ khi có office hour được author.
- Thiếu eligibility: hiển thị từng điều kiện thiếu, nguồn dữ liệu và recovery; nếu không có recovery trong calendar, ghi rõ. NPC không tự biết điểm/topic nếu chưa được chia sẻ hoặc không có quyền xem record.

## EV_Y1_FINAL_EXAM — Thi và chốt môn

- Category scheduled academic; W10, `C_NMT_FINAL` window `[08:00,09:00)`, LOC_CLASS, enrolled và eligible. Assessment định nghĩa question IDs, topic mapping, timer, rubric và điểm pass trước khi được đánh giá. Nếu thiếu định nghĩa, trạng thái `blocked_data`, không tạo kết quả pass/fail hay credit.
- `start`: tạo attempt và lưu state; `submit`: unanswered sai; `exit`: xác nhận tiếp tục hay nộp; world time pause trong bài, timer riêng được lưu. Nộp chỉ một lần. Miss window: `missed_exam`; không sinh điểm 0 trừ khi course definition quy định rõ.
- Chỉ resolver course hoàn chỉnh mới chuyển `enrolled → passed|failed`; `creditAwarded` chống cộng hai lần. Fail/missed giữ evidence; hiển thị lịch học lại nếu có. Các course khác resolve độc lập, không ép cả năm failed vì một môn.

## EV_Y1_HOTPOT — Bữa cuối năm

- Fixture D71 và định nghĩa chi tiết về mời khách, chuẩn bị, bữa hỗ trợ, interruption, memory và callback: xem [59](59_Hotpot_nam_1.md). Không yêu cầu pass course; từ chối không cản chuyển năm. Lời thoại Long chọn theo `44`.

## EV_Y1_YEAR_RESOLVER — Chuyển Năm 2

- Category scheduled; D84 sau các course windows đã author. Preview, chuyển trạng thái và điều kiện chặn khi thiếu dữ liệu được chi tiết trong [64](64_Chuyen_nam_1_sang_2.md).
- `course.status=unresolved|blocked_data` phải hiện riêng; không chuyển ngầm sang pass, fail hoặc cấp credit. Không tự sinh lớp/vắng trong khoảng skip. Snapshot giữ course/topic, ledger, tiền/energy, relationship, conflict, memory, location unlock, project, event instance và RNG; công bố Year 2 sau save thành công.
- QA đề xuất: không có Long khi nhận phòng, từ chối ăn, không đủ tiền, chưa chọn giường, đề thi thiếu, thi lỡ, conflict còn mở, bữa lẩu không khách, cancel/save failure, credit không cộng hai lần. Chưa thực thi.
