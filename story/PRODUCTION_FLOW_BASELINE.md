# Production flow baseline — 25/10/2026

Đây là baseline dùng để thực thi plan asset sau FULL_FLOW_AUDIT_BEFORE_GENERATION.md.
Các mô tả chi tiết trong MASTER_GAME_FLOW.md vẫn là tài liệu cốt truyện; khi mâu thuẫn về transition, môn, trang phục hoặc asset, áp dụng baseline này và REMAINING_ASSET_INVENTORY.md.
Đây là quyết định thiết kế cho game, chưa phải tuyên bố đã triển khai gameplay.
Định vị, agency, variety, thời lượng và gate trải nghiệm được quy định tại [PLAYER_EXPERIENCE_PLAN.md](PLAYER_EXPERIENCE_PLAN.md); tài liệu đó không tự thay đổi graph/mốc canon trong baseline này.

## Các quyết định áp dụng

- Tuyến chính đủ45mission. Lựa chọn nhỏ (NO, thức dậy, xin hỗ trợ) có phản ứng/hậu quả thật rồi hội tụ; không thêm ending mới.
- Bạn cùng lớpH6 thay Đạm ở M06. Đạm phản ứng tại404. M07 hụt xe có điểm đón phụ/chuyến sau.
- Sau lẩu M08 mở đăng ký M09, sau đó mới M10. Không dùng nhánh demo tự nhảy năm để xác nhận hoàn thành game.
- Graph game: KTLT→CTDL→HĐH. Năm3 có bước passKTLT để mởCTDL trước khi nợCTDL. GT2 đãpass thì không thi trả nợ lại năm5.
- Credits cân bằng hư cấu:32/50/78/118/136/150. M27 thiếu18 so với136 để được làm đồ án, không thiếu18 so với150 để tốt nghiệp. Không trình bày đây là quy định thật của trường.
- M35 trả nợCTDL kỳ9 rồi HĐH kỳ10, không vi phạm môn tiên quyết. M36 mới mở đăng ký đồ án M37.
- Một đề tài xuyên suốt: hệ thống tự động hóa và chẩn đoán lỗi thiết bị mạng.
- Đạm chưa bay trước lần tiếp tế M34. Ba người mới thay ba bạn. Cuối M44 giữ đồ người mới và chỉ dọn góc Lươn.
- Hội đồng5người: visualGVHD reuse teacher +4 giám khảo mới. Không cấp A trước khi hoàn thành bảo vệ.
- M44 gọi mẹ→gói đồ→tắt đèn→trả khóa. M45 nhận bằngA5→hội ngộBìnhĐông→đi cùng anh em→credits mới Finished.
- M13 hạn23:59:59, submit mẫu23:59:41. M42 bắt đầu22:45/hạn23:45, receipt mẫu23:44:32; số giờ dùng data, không bake ảnh.
- M05 và M22 dùng chung4pose ngồi ghế C02; các pose ngồi sàn cũ chỉ dùng khi phù hợp.
- Cài đặt/job/tín chỉ không lặp vô hạn để bù thời gian:3job đại diện + montage có ledger qua nhiều tuần; ví/năng lượng có recovery.

## Graph mission

| Mission | Entry | Action/điều kiện ra | Cầu nối |
|---|---|---|---|
| M01 | Xuống xe khách | Né người mời xe, tới trạm, mua vé | Đến sảnh KTX; có hỗ trợ nếu cạn tiền |
| M02 | Sảnh KTX | Nộp giấy, nhận chìa khóa và phòng 404 | Quản lý chỉ đường lên tầng 4 |
| M03 | Mở cửa 404 | Xí giường, đặt balo, siết ốc, cắm quạt | Thấy bụi và nhận danh sách mua đồ dọn |
| M04 | Phòng bụi | Mua bộ vệ sinh; lau 4 điểm trong 60 giây | Bạn đến lần lượt rồi rủ ăn; timeout chỉ retry phần dọn |
| M05 | Cả nhóm đói | Chọn cơm trong ngân sách và làm quen | Lịch học hiện trên điện thoại; về ngủ |
| M06 | Sáng đi học | Balo/WC/xe/H6; warm-up 55; học rồi về kể Đạm | Hẹn một buổi học khác; chưa cấp card cuối năm |
| M07 | Một sáng khác, 07:20 | Xếp hàng WC; đón xe hoặc tuyến dự phòng; điểm danh | Chuỗi ngày đại diện và kết quả thi: 32 tín chỉ; rủ lẩu |
| M08 | Đã 32/150, không nợ | Lấy bếp, cắm điện, thêm đồ, ăn, cụng ly | Ngủ; thẻ chuyển kỳ 3 và tin đăng ký môn mở M09 |
| M09 | Ngày đăng ký, 08:59 | Refresh, chọn lớp, xác nhận thời khóa biểu | Hiện lớp 06:45; tuần học đại diện dẫn tới M10 |
| M10 | Một tối 23:40 | YES chơi thêm hoặc NO đi ngủ thật | Sáng hôm sau khác năng lượng/điểm danh rồi hội tụ |
| M11 | Báo thức 06:30 | Dậy hoặc Snooze, ghi hậu quả riêng | Một tối khác nấu lẩu M12; không coi đây là buổi thi |
| M12 | Nghe chìa khóa khi nấu | Rút phích, cất nồi, giả ngủ trong 10 giây | Nhắc deadline C++; bị bắt có cảnh cáo hoặc retry |
| M13 | Tin nhắc bài C++ | Compile, tìm lỗi, patch, test, submit trước 23:59:59 | Điểm thành phần thấp; nhận lịch thi cuối kỳ |
| M14 | Ngày thi, đủ tư cách | Tới phòng, làm phần kiến thức đã học, nộp | Tin hẹn công bố điểm; không cấp F ngay từ ngủ quên |
| M15 | Portal công bố điểm | Xem 3 F và các môn bị khóa cụ thể | Mở hóa đơn học lại rồi nhận cuộc gọi mẹ |
| M16 | Hóa đơn, mẹ gọi | Trả lời theo lựa chọn; kết thúc cuộc gọi | Tin khách cạnh phòng có máy hỏng mở job đầu |
| M17 | Khách mang laptop | Sao lưu giả lập, USB, cài, bàn giao, nhận 50k | Khách giới thiệu thêm; Lươn muốn treo biển |
| M18 | Hoàn thành job đầu | Dán biển và nhận đơn mới | Job board và lịch học lại cùng được mở |
| M19 | Lịch học/ca sửa máy | Job đại diện, nghỉ/ăn, montage thu chi, nộp học phí | Kỳ 5 pass KTLT/VL1; kỳ 6 mở CTDL; khách nữ nhắn |
| M20 | Crush nhờ sửa | Scan virus, sửa và trả máy, miễn công | Tin hẹn tới quán; mượn áo Long |
| M21 | Tới quán theo tin | Gặp bạn trai crush và sửa máy theo checklist | Về trả áo; bạn nhận ra buồn và rủ Hồ Đá |
| M22 | Nhóm hẹn Hồ Đá | Ngồi, uống trà, ăn và nói chuyện không timer | Về ngủ; nhận tin điểm cuối năm |
| M23 | Portal năm 3 | Xem 78 tín chỉ; GT2 pass, CTDL fail | Đạm nhờ mang sách tới lab |
| M24 | Sách từ Đạm | Đến lab HCMUS, giao sách, nghe thảo luận | Về 404; Long nhờ chỉnh CV |
| M25 | CV trên laptop | Chỉnh định dạng, xuất và gửi CV | Vài ngày sau có mail đậu; Tộ chuẩn bị ca sáng |
| M26 | Buổi sáng bạn đi làm | Tiễn Tộ; xem lịch bạn bè và phòng dần vắng | Tin xét điều kiện đồ án của khóa |
| M27 | Đợt xét đồ án | Nộp đơn; thấy 118/136 và thiếu 2 tiên quyết | Bạn nhờ chụp ảnh chung; không cho đăng ký lặp vô hạn |
| M28 | Hẹn chụp tại ĐHQG | Chụp 3 bạn và trả lời phụ huynh | Về 404; Long báo dọn đầu kỳ 9 |
| M29 | Long đóng thùng | Bê đồ, chào, giường trống rồi người mới 1 vào | Một ngày khác Tộ báo chuyển trọ |
| M30 | Tộ rời đi | Nhận quà, ôm/chào; người mới 2 đến | Đạm nhắn hoàn tất thủ tục và ngày rời KTX |
| M31 | Đạm ở cửa | Báo sắp đi và nói chuyện với Lươn; chưa rời phòng, chưa có người mới 3 | Đêm nhìn icon game; tiếp tục lịch trả nợ |
| M32 | Chọn laptop | Uninstall, giữ đúng nút, hoàn tất | Đặt báo thức 06:30 và lịch trả nợ; chưa mở đồ án |
| M33 | Ngày mới 06:30 | Bật dậy, WC, đi học đúng giờ | Chuỗi ngày học đại diện; khuya có tiếng gõ cửa |
| M34 | Đang ôn lúc 01:00 | Bạn cũ tiếp tế lẩu/tài liệu; Đạm vẫn ở phòng trong đêm đó | Sáng sau Đạm thực sự rời; người mới 3 chỉ vào sau khi giường trống; montage sang kỳ thi |
| M35 | Đã ôn, đúng học kỳ | Kỳ 9 pass CTDL; kỳ 10 học/thi HĐH; hết nợ | Kết quả cập nhật; portal xét điều kiện |
| M36 | Portal có đủ điểm | Xác nhận 136/150 và hết nợ tiên quyết | Form đề tài và lịch GVHD đầu năm 6 |
| M37 | Lịch hẹn GVHD | Gặp thầy, chọn đề tài, đăng ký | Checklist nghiên cứu và nguồn tài liệu |
| M38 | Đề tài duyệt | Tra cứu, ghép sơ đồ, xác nhận thiết kế | Backlog module trên laptop |
| M39 | Backlog code | Sửa code/chạy test; demo đầu chạy được | Đặt lịch demo 1; ngủ và tới GVHD |
| M40 | Demo 1 | Chạy demo, nhận feedback, ghi việc sửa bảo mật | Về sửa sơ đồ/code; test mới pass để mở demo 2 |
| M41 | Demo 2 đạt | Duyệt, format 120 trang, in và đóng bìa | Nhắc hạn nộp; kiểm thử trước khi upload |
| M42 | Crash trước hạn | Tái hiện lỗi, đọc log, patch, test, submit | Receipt và backup; nhận lịch bảo vệ sau đó |
| M43 | Receipt và lịch hội đồng | Sơ mi, cắm máy chiếu, thuyết trình, Q&A, đạt A | Sai thì retry tại checkpoint/tập dượt; đạt mới mở M44 |
| M44 | Kết quả đạt | Gọi mẹ, gói đồ riêng, tắt đèn, khép cửa, trả khóa | Thư mời chuyển tới ngày lễ; chưa đánh Finished |
| M45 | Thư mời lễ | Nhận bằng A5, gặp gia đình/bạn; đi Bình Đông, khoác vai | Fade, credits, lưu hoàn thành, quay menu |

## Trạng thái triển khai

Flow trên là mục tiêu. Scene Room404_MVP hiện vẫn có các stage demo; catalog/import/gallery không chứng minh flow này đã chạy. Kiểm tra full placeholder route, hệ thống quest/economy/save và gameplay integration còn cần hoàn thành theo goal.
