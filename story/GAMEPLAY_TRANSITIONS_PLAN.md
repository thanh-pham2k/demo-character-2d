# Gameplay transitions plan — thời gian, địa điểm và nhịp campaign

Tài liệu này quy định cách chuyển giữa hoạt động, ngày, học kỳ và địa điểm. Thứ tự mission/canon vẫn theo [Production flow baseline](PRODUCTION_FLOW_BASELINE.md). Runtime năm nhất hiện có lịch đại diện và preview khi ngủ; campaign M10–M45 hiện chủ yếu có thứ tự event/bridge, chưa có timestamp cho từng bước.

## 1. Mô hình thời gian

Campaign dùng **ngày đại diện và montage**, không chạy đồng hồ mô phỏng liên tục suốt sáu năm.

- Dùng giờ chính xác khi deadline, báo thức, xe buýt hoặc một lựa chọn phụ thuộc vào giờ.
- Dùng ngày/tuần/học kỳ và montage cho khoảng thời gian không chơi trực tiếp.
- Nếu ngày dương lịch chưa được author, dùng nhãn tương đối nhất quán như “HK9 • tuần 2 • Thứ Ba”; không tự suy ra ngày tháng thật.
- Không thể hiện time-skip nhiều tuần như thể nhân vật chỉ ngủ một đêm.
- Màn hình chuyển thời gian cho biết ngày/giờ nguồn, **thời điểm của sự kiện đích**, thời lượng thực tế đã trôi và các thay đổi quan trọng. Số ngày là kết quả tính từ hai mốc, không phải thời lượng mặc định.
- Transition đi tới thời điểm của sự kiện kế tiếp cần người chơi chú ý/tương tác; không tự dừng ở mốc 30 ngày và không vượt qua một lựa chọn gameplay khác. Nếu giữa hai mốc có biến cố không cần tương tác, montage/summary phải thể hiện những thay đổi quan trọng.
- “30 ngày” trong mockup là ví dụ về một khoảng nhảy có thể xảy ra, không phải giới hạn mỗi chặng hay quy tắc chia montage. Khoảng dài hơn vẫn tới đúng thời điểm sự kiện đích; chọn ngủ/chuyển thời gian không bắt người chơi xác nhận từng ngày.
- Mỗi transition phải có nguồn, sự kiện đích và thời điểm/thời lượng được ghi trong tài liệu hoặc dữ liệu có cấu trúc; thời lượng được suy ra từ hai mốc. Không để thời gian chỉ tồn tại trong câu `bridge` tự do.

## 2. Mẫu chuyển thời gian ba bước

### A. Preview trước khi commit

Hiện mốc nguồn → mốc đích, sự kiện quan trọng sẽ bị lỡ, thay đổi đã biết và lựa chọn tiếp tục/hủy. Không liệt kê mọi event nhỏ; chỉ hiện thay đổi có ảnh hưởng gameplay.

### B. Montage / chuyển cảnh

Giữ nhịp ngắn. Dùng 2–4 hình/hoạt động đại diện cho khoảng thời gian: lịch lật, lớp học, ca làm, cuộc gọi, phòng 404 đổi trạng thái. Với qua đêm thông thường dùng hoạt ảnh ngủ; với nhiều ngày dùng montage lịch/tuần.

### C. Summary và điểm đến

Hiện ngày/giờ/địa điểm mới, thay đổi state đã commit và objective kế tiếp. Khôi phục quyền điều khiển tại safe location; có checkpoint ở trước hoặc sau transition theo convention của runtime.

Ví dụ:

```text
TỪ: [thời điểm hiện tại] • 23:40
ĐẾN: [tên và thời điểm sự kiện kế tiếp] • 07:20
[thời lượng tính từ hai mốc; ví dụ: 30 ngày]

• Học kỳ mới bắt đầu
• Hai deadline đã đến gần
• Phòng 404 thay đổi

[Ở lại]  [Chuyển thời gian]
```

## 3. Loại transition

| Loại | Dùng khi | Trình bày |
|---|---|---|
| Đi lại có gameplay | Chuyến đầu, đường đi có lựa chọn/rủi ro | Cho đi bộ/đi xe; giữ điểm xuất phát, đích và fare rõ ràng |
| Đi lại lặp | Tuyến đã quen, không tạo quyết định mới | Montage hoặc chuyển ngắn có nhãn địa điểm |
| Qua đêm | Ngủ tới sáng kế tiếp | Giường/ánh sáng/âm thanh; giờ đích và việc có thể bị lỡ được preview |
| Vài ngày hoặc vài tuần | Lịch học, job hoặc chờ kết quả | Đi tới thời điểm sự kiện kế tiếp; thẻ ngày + montage tóm tắt 1–3 thay đổi state |
| Nhiều tuần/tháng | Mốc học kỳ, kết quả, chuyển nhà hoặc lịch hẹn | Đi thẳng tới thời điểm sự kiện đích; montage/summary giữ đúng các biến cố quan trọng và tổng thời gian thực tế |
| Kết thúc chapter | Chia tay, đủ điều kiện đồ án, tốt nghiệp | Giữ nhịp cảm xúc; không chen HUD số liệu không liên quan |

## 4. Cầu nối thời gian theo campaign

| Đoạn | Cầu nối cần thể hiện |
|---|---|
| M01–M05 | Cùng ngày nhập học; đi xe, làm thủ tục, lên phòng, dọn và ăn. Chỉ cắt cảnh khi người chơi đã hoàn thành hành động tại chỗ. |
| M05–M08 | Qua đêm tới ngày học; M06 và M07 là các ngày đại diện khác nhau. Sáu môn/12 lượt xe đi qua bằng chặng học–thi có nhãn học kỳ; cuối chặng hiển thị sổ vé, 32 tín chỉ và rủ lẩu. |
| M08→M09 | Lươn ngủ một đêm; thẻ mở học kỳ 3 và đưa tới đăng ký 08:59. Không gộp một học kỳ vào hoạt ảnh ngủ. |
| M09→M10 | Montage tuần học đầu, cho thấy lớp 06:45 và lịch sáng trước khi đến tối 23:40. |
| M10→M11 | Giữ cùng một đêm: YES chơi đến 04:32 rồi còn dưới hai giờ tới báo thức 06:30; NO đi ngủ lúc 23:40. Sau đó wake/snooze cho kết quả giờ dậy và điểm danh nhất quán. Snooze tới 11:47 phải là một time-lapse rõ ràng. |
| M11→M12 | Ghi rõ đây là tối của một ngày khác, không chuyển thẳng từ chuông sáng sang bếp lẩu. Báo trước ngày/giờ kiểm tra KTX. |
| M12→M15 | Tách đêm giấu bếp khỏi hạn C++ 23:59:59; có thẻ “ngày hôm sau”/hạn nộp. Sau deadline dùng montage tới ngày thi, rồi chờ vài ngày tới portal công bố điểm. Show recap các nguyên nhân học vụ trước khi hiện ba F cố định theo canon. |
| M15→M19 | Cuộc gọi mẹ gắn với hóa đơn sau khi có kết quả; khách sửa máy xuất hiện vào ngày sau, không spawn ngay giữa cuộc gọi. Ba job đại diện và ledger được tóm tắt qua học kỳ 5–6. |
| M19→M28 | Dùng thẻ kết quả học kỳ 5/6, ngày nhắn crush, cuộc hẹn, Hồ Đá, rồi mốc năm 4. Những câu “vài ngày sau” cần có ngày hoặc nhãn montage; không cho CV/job/friend departures diễn ra như cùng một buổi. |
| M29→M34 | Mỗi người chuyển đi rồi mới lấp giường đó. Sau M30 phòng còn Lươn, Đạm và hai người mới. M31 là Đạm báo sắp đi, chưa rời và chưa có người mới thứ ba. M34 là lần tiếp tế cuối của bạn cũ; Đạm rời vào sáng sau đó, người mới thứ ba chỉ vào sau khi giường trống. |
| M32→M35 | M32 đêm uninstall → M33 sáng kế tiếp → các ngày học/repair/chuẩn bị đồ án và internship nếu hợp lệ → tiếp tế 01:00. M35-A CTDL kỳ 9/10; preview riêng cuối Năm 5 → kỳ 11 Năm 6, rồi M35-B đăng ký/học/thi HĐH chỉ sau CTDL PASS. Không ép trượt/thi lại môn đã PASS. |
| M35→M37 | Đã ở đầu Năm 6: chờ điểm môn cuối → M36 xét 136/150 + đủ tiên quyết → lịch GVHD M37. Có thời gian học, ôn, thi và chờ điểm thật trước đăng ký đồ án; chưa đủ thì recovery, không skip để nhận form. |
| M37→M43 | Ghi rõ tuần/tháng trôi qua giữa nghiên cứu, code, demo 1, sửa, demo 2 và nộp báo cáo. M39 time-lapse 22:43–05:26 là một đêm; M40/41 là các mốc hẹn riêng. M42 dùng deadline 22:45–23:45 theo baseline; receipt mở lịch bảo vệ sau vài ngày. |
| M43→M45 | Bảo vệ → kết quả → đối soát học vụ/học phí cuối/ca và đồ khách → M44 gọi mẹ đúng trạng thái, dọn riêng góc Lươn, nhìn lại, khép cửa/trả khóa. Thiếu tiền/điều kiện thì recovery có nguồn, không tự mở nhận bằng. M45 ngày lễ riêng → A5 → Bình Đông có travel → flashback history → credits/Finished → tease công việc chỉ nếu đã có lời hẹn hợp lệ. |

## 5. Ràng buộc continuity

- Phòng KTX có bốn chỗ ở: Lươn và ba bạn; mỗi người mới chỉ nhận chỗ sau khi một người cũ rời đi.
- Nếu người cũ thăm lại lúc 01:00, thể hiện họ là khách, có lý do vào KTX và không khiến người mới phải tham gia một bữa tiệc trái với lịch sinh hoạt của họ. Dùng khu sinh hoạt chung/hành lang nếu cảnh cần tụ tập.
- Không dùng lại cùng một ngày/giờ cho hai việc ở địa điểm khác nhau nếu chưa có travel time hoặc time-skip.
- Deadline, snooze, bus, lẩu khuya, chuyển nhà, demo và bảo vệ cần có thứ tự giờ/ngày nhất quán.
- Save/retry không được lặp lại cảnh chuyển đã commit hoặc cộng/trừ state hai lần.
- Lượt tập dượt bảo vệ có thể retry tại checkpoint trước buổi hội đồng; kết quả của buổi bảo vệ chính thức không tự chạy lại như thể hội đồng vừa hỏi lại ngay lập tức.

## 6. Kiểm tra mỗi transition

Trước khi đưa transition vào scene, xác nhận: nguồn, trigger, thời gian trôi, kiểu biểu diễn, hậu quả bị bỏ lỡ, điểm đến, state sau chuyển và checkpoint. Các đoạn dài cần cho người chơi biết điều gì đã thay đổi; không chỉ đổi cảnh và cập nhật objective.

## 7. Giới hạn arc cuối

Năm 4 không có tốt nghiệp; Năm 5 không mở đời nhân viên full-time. Đồ án nhỏ/chuẩn bị không thay đăng ký đồ án sau M36. Năm 6 nối môn cuối → bảo vệ → học phí còn nợ → rời 404 → tốt nghiệp. Xem [payoff Năm 6](YEAR_SIX_PAYOFF_PLAN.md). Bạn cũ đã rời KTX Năm 5 nên ở lễ là hội ngộ; Đạm cần lịch về dự được author. Flashback không thay gameplay đóng phí/dọn phòng/trả khóa và không xóa đồ của người mới.
