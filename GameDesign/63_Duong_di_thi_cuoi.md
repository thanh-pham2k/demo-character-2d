# Đường tới thi cuối C_NMT — fixture D64 v0.1

Đây là tuyến giả lập cho final C_NMT; không tự nhân bản sang ngày khác. Gắn lịch thi `14`, chuẩn bị `62`, boundary boarding `29`, timer `40` và save `43`. Chưa chứng minh runtime hoặc cân bằng.

## Tuyến và entry

Khi `FINAL_NOTICE_NMT` công bố D57, player đã enrolled biết `FINAL_NMT` và các tuyến sau; không tiết lộ thông tin chưa được công bố của các event khác. Nếu route đóng hay definition thiếu thì preview ghi unknown/blocked, không giả định đi được.

| ID | Route / giờ | Thời gian, E, tiền |
|---|---|---|
| `D64_ROOM_BUS` | ROOM→BUS, ngược lại cùng cost | 10 phút, −1 E, 0 M |
| `D64_BUS_0740` | cửa 07:35, rời 07:40, đến H6 07:45 | chờ đến departure, lên xe 3 M, 5 phút xe |
| `D64_BUS_0750` | cửa 07:45, rời 07:50, đến H6 07:55 | như trên |
| `D64_BUS_H6_WALK` | BUS→H6 | 40 phút, −4 E, 0 M |
| `D64_H6_CLASS` | H6→CLASS | 5 phút, −1 E, 0 M |
| `D64_SUPPORT` | BUS [07:35,09:00), một lần | player yêu cầu xe hỗ trợ tới H6 mất 30 phút, −1 E, 0 M; phải hoàn tất không muộn hơn 09:00 |

Đây là hỗ trợ hư cấu, có operator tại BUS cho occurrence này; không tự suy rộng sang các ngày. Route E phải đủ toàn lượng trước start; nếu không đủ, nêu lý do và dùng nghỉ tại trạm theo `55` nếu điều kiện của activity được đáp ứng. Không tự hồi E, ứng tiền, tạo chuyến thứ ba hoặc teleport. Player chọn cụ thể chuyến; tại departure boarding decision theo `29`, chỉ ghi `missed_bus_id` cho chuyến đã chọn nhưng lỡ.

## Vào phòng thi

`FINAL_NMT`: D64 [08:00,09:00), CLASS. Nếu `C_NMT enrolled` và có đầy đủ question/rubric/timer policy, player chủ động start assessment khi đang tại CLASS trong window; chọn start lúc 08:00 hoặc sau đó nhưng **trước** 09:00 đều hợp lệ trong fixture và dùng cùng exam, không thêm phạt trễ. Đến 09:00 chưa start → `missed_final`; xét điểm 0 có reason theo `14`. Đến đúng 09:00 không start được. Đang làm bài đã start thì world clock pause theo `40`, không tự thành missed lúc thực tế qua 09:00. Missing data → `blocked_data`, không ghi missed vì lỗi content. Không nhầm `đến đúng giờ` với `đã start`: player vẫn phải xác nhận dự thi.

Travel không cấp attendance hay exam attempt. Tại trạm, UI hiển thị arrival dự kiến tới CLASS theo route được chọn, cost và cảnh báo start sau đóng cửa; không khẳng định người chơi sẽ dự thi. Mỗi route cần một runId, giữ elapsed/cost nếu ngắt theo `29`; chỉ commit location khi tới nơi. Nếu chưa có dữ liệu vị trí giữa đường thì không cung cấp nút ngắt route đó. Boarding và phí dùng transaction `29`/`43`.

## Traces tính tay

1. ROOM 07:30 → BUS 07:40 → board 07:40 → H6 07:45 → CLASS 07:50 → đợi, start 08:00. Tổng 3 M và 2 E cho hai đoạn đi bộ, chưa gồm hành động trước đó.
2. Đang tại BUS 07:55 → chọn support → H6 08:25 → CLASS 08:30, E −2, 0 M → start 08:30 hợp lệ. Đây là cơ hội cứu chuyến thi, không tự ghi đã thi.
3. BUS 08:15 → đi bộ → H6 08:55 → CLASS 09:00, E −5 → đã hết window; final missed nếu các điều kiện ghi missed hợp lệ. Từ BUS 08:14 cùng route → CLASS 08:59, có một phút campaign để chủ động start; timer bài thi là real-time riêng và world clock pause.
4. BUS 08:30, support tới H6 09:00, CLASS 09:05: được dùng nếu còn quyền hỗ trợ nhưng UI phải cảnh báo không kịp final; không thể tự cho start.

Không phải mọi E/vị trí đều có phương án đến kịp; player có thể lỡ thi, giữ story/relationship và theo recovery `30` khi đủ điều kiện. Chưa chứng minh route tới trạm từ mọi location, availability khác hoặc khả năng toàn campaign.

## QA đề xuất

Đến BUS chính xác giờ rời; chưa/đã chọn xe; thiếu 3 M, E=0/1/5; support hết giờ; đến CLASS 08:59/09:00; chưa start; app pause trong attempt; missing rubric; save lỗi tại boarding/arrival; double fare; retake chỉ sau eligibility. Đây là test cases, chưa chạy.
