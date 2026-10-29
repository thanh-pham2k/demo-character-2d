# NPC Bible — bản nháp thiết kế v0.1

Tên và hướng truyện là đề xuất cho dự án mới. Đây là hồ sơ/graph, chưa phải event database production-ready. NPC có lịch, nhu cầu và thông tin riêng: không tự biết việc player chưa kể; không tự xuất hiện trong memories chưa xảy ra. Mỗi cảnh đánh dấu ID đề xuất, cần bổ sung window, điều kiện, choice, effect và dialogue trong event database.

## LONG — bạn cùng phòng

**Vai trò:** roommate và bạn chơi game; **nét chính:** hoạt bát, hòa đồng. **Muốn:** có bạn đồng hành nhưng vẫn giữ không gian riêng. **Sợ:** bị xem là người kéo bạn sa sút. **Điểm yếu:** dùng lời đùa để tránh đối thoại khó. **Bối cảnh:** cùng chuyển tới KTX; có lịch học và bạn bè riêng. **Chủ đề:** thân thiết không đồng nghĩa luôn đồng ý.

- Y1: gặp tại 404, ăn/chơi cùng, va chạm việc ngủ và đèn; có thể chưa thân hoặc còn bất đồng.
- Y2: tham gia hoạt động riêng; lời mời có hạn, hai người có thể tìm lịch gặp khác.
- Y3: mục tiêu khác nhau, cơ hội hợp tác hoặc từ chối một lời nhờ.
- Y4: internship/lịch tốt nghiệp khác khiến việc gặp khó hơn.
- Y5–6 nếu player còn học: Long có đời sống riêng; có thể thăm, liên lạc hoặc xa cách tùy quan hệ và availability, không mặc định đã đi làm.
- Xung đột: đèn sáng khi Long cần ngủ → thỏa thuận, trì hoãn hoặc bất đồng; giải quyết chỉ khi có outcome được author.
- Kết: thân thiết, bạn bình thường, xa cách hoặc hòa giải; predicate dựa trên memories, trust, conflict và sự kiện thực, không chỉ một tổng điểm.

**15 scene seeds:** `L01` gặp; `L02` bữa đầu; `L03` chơi trước lớp; `L04` chuyện xe; `L05` trao đổi sau giờ học; `L06` ôn giữa kỳ; `L07` nhờ giúp; `L08` từ chối hợp lý; `L09` đèn khuya; `L10` hậu quả hôm sau; `L11` thỏa thuận; `L12` lời mời CLB; `L13` lịch riêng; `L14` gặp sau internship; `L15` callback bữa lẩu hoặc lời tạm biệt. L04 chỉ dùng thông tin Long biết; L11 không tự diễn ra nếu player né tránh; L15 có biến thể chưa thân.

## MINH — bạn học

**Vai trò:** bạn học có tham vọng; **nét chính:** kỷ luật; **muốn:** làm tốt và được công nhận; **sợ:** phụ thuộc người khác; **điểm yếu:** khó chia sẻ công việc. **Bối cảnh:** gặp qua lớp hoặc hoạt động học; **chủ đề:** cạnh tranh và hợp tác có thể cùng tồn tại.

- Y1: gặp tại lớp hoặc một dịp học lại; trao đổi topic yếu.
- Y2: project nhóm có thể tranh luận chia việc; nếu player không tham gia thì Minh vẫn có nhóm riêng.
- Y3: cơ hội cùng hackathon hoặc so sánh định hướng, không mặc định đối thủ.
- Y4: lựa chọn tham gia review hồ sơ hoặc tôn trọng ranh giới thời gian.
- Y5–6: gặp lại qua buổi học bù, nhắn tin hoặc vắng mặt theo lịch và quan hệ.
- Conflict: kết quả project không như mong đợi; có thể góp ý riêng, đổ lỗi, chia lại việc. Kết: bạn học, đồng đội tin cậy, đối thủ tôn trọng hoặc mất liên lạc.

**12 scene seeds:** `M01` gặp; `M02` trao đổi bài; `M03` hỏi lại topic; `M04` giữa kỳ; `M05` mời nhóm; `M06` chia việc; `M07` deadline; `M08` feedback; `M09` hackathon; `M10` CV review; `M11` lựa chọn internship; `M12` callback sau tốt nghiệp. Nếu M01 lỡ, M02/M03 cần cửa gặp thay thế; không được gọi player là bạn thân nếu chưa gặp.

## AN — bạn từ hoạt động xã hội

**Vai trò:** người kết nối xã hội; **nét chính:** chủ động, sáng tạo; **muốn:** xây một nhóm có tiếng nói riêng; **sợ:** tình bạn chỉ tồn tại lúc vui; **điểm yếu:** nhận quá nhiều việc. **Bối cảnh:** gặp qua sự kiện mở từ Y2 hoặc cơ hội thay thế được author; **chủ đề:** sự hiện diện và ranh giới.

- Y1: chưa bắt buộc gặp; không tạo memory giả.
- Y2: CLB/hoạt động cộng đồng; player có thể làm bạn hoặc giữ khoảng cách.
- Y3: một kế hoạch cộng đồng trùng với việc học hoặc project; An có quyền từ chối nếu bận.
- Y4: quan hệ thay đổi khi nơi làm và lịch khác nhau.
- Y5–6: gọi điện, sự kiện cộng đồng hoặc không gặp tùy state; romance là nhánh tùy chọn, không mặc định.
- Conflict: player nhận lời giúp rồi không đến; có thể giải thích, xin lỗi hoặc giữ im lặng. Kết: bạn, người thân thiết, quan hệ tình cảm nếu hai bên chọn, hoặc xa cách.

**12 scene seeds:** `A01` gặp; `A02` giới thiệu hoạt động; `A03` nhiệm vụ nhỏ; `A04` trễ hẹn; `A05` trao đổi ranh giới; `A06` lời mời; `A07` sự kiện cộng đồng; `A08` từ chối; `A09` hội thoại riêng; `A10` bận công việc; `A11` liên lạc sau xa cách; `A12` callback ngày tốt nghiệp. A04 chỉ mở nếu đã nhận lời và vắng; romance phải có lựa chọn thuận tình riêng.

## NAM — mentor / đàn anh

**Vai trò:** mentor kỹ thuật; **nét chính:** thực tế; **muốn:** giúp đàn em tự giải quyết vấn đề; **sợ:** hứa giới thiệu khi chưa hiểu năng lực; **điểm yếu:** đôi khi góp ý quá trực diện. **Bối cảnh:** gặp qua CLB, review project hoặc buổi hướng nghiệp; **chủ đề:** xin giúp đỡ không thay thế nỗ lực.

- Y1: có thể là nhân vật nền; không yêu cầu gặp.
- Y2: có cơ hội hỏi về CLB hoặc project; nếu không tham gia, có thể gặp qua review mở.
- Y3: xem project, giới thiệu kỹ thuật hoặc chỉ cách tự tìm cơ hội.
- Y4: góp ý internship, có thể từ chối referral nếu thiếu evidence.
- Y5–6: hướng dẫn chuyển nghề, hoặc chỉ xuất hiện trong một thư chung nếu không quen.
- Conflict: player yêu cầu referral khi chưa chuẩn bị; chọn hỏi feedback, nóng giận hoặc tự cải thiện. Kết: mentor tin cậy, quan hệ nghề nghiệp thông thường hoặc không liên lạc.

**12 scene seeds:** `N01` gặp; `N02` hỏi đường học; `N03` code review; `N04` phản hồi khó nghe; `N05` sửa project; `N06` từ chối referral; `N07` hỏi rõ yêu cầu; `N08` event nghề; `N09` mock interview; `N10` cơ hội thực tập; `N11` career pivot; `N12` callback offer. Referral là cơ hội phỏng vấn, không phải offer chắc chắn.

## HẠNH — giảng viên

**Vai trò:** giảng viên và người hướng dẫn học thuật; **nét chính:** thẳng thắn, có nguyên tắc; **muốn:** sinh viên hiểu bài và tự nhận trách nhiệm; **sợ:** giúp sai cách khiến sinh viên lệ thuộc; **điểm yếu:** ít thời gian và đôi khi khó gần. **Bối cảnh:** dạy học phần nền tảng, có giờ tư vấn được lên lịch; **chủ đề:** yêu cầu hỗ trợ là một kỹ năng.

- Y1: tiết đầu, phản hồi đi muộn và cơ hội hỏi bài; vắng buổi đầu vẫn có office hour.
- Y2: phản hồi project nhóm theo evidence.
- Y3: đề xuất hướng nghiên cứu cho player có dữ liệu phù hợp; không ràng buộc mọi career path.
- Y4: review proposal/đồ án khi được phân công, không tự thành người hướng dẫn mọi run.
- Y5–6: giúp lên kế hoạch hoàn tất học phần nếu có quan hệ và thời gian, hoặc giới thiệu kênh hỗ trợ chung.
- Conflict: player bỏ deadline rồi yêu cầu ngoại lệ; có thể đưa bằng chứng, chấp nhận quy định hoặc tìm recovery. Kết: quan hệ tôn trọng, xa cách hoặc hợp tác học thuật.

**12 scene seeds:** `H01` tiết đầu; `H02` đi muộn; `H03` office hour; `H04` bài chưa nộp; `H05` feedback; `H06` project review; `H07` bảo vệ lựa chọn; `H08` buổi hướng nghiệp; `H09` đề tài; `H10` proposal; `H11` học lại; `H12` lời chào tốt nghiệp. H02 chỉ khi player đến muộn và Hạnh có mặt, không tự biết sự kiện xe.

## Nhân vật phụ — danh sách ban đầu

| ID | Vai trò | Mong muốn/lịch độc lập | Cơ hội tương tác | Điều kiện tránh giả định |
|---|---|---|---|---|
| S01 | Quản lý KTX | Hoàn thành ca trực | Thủ tục, báo sự cố | Không ép nhân vật xuất hiện ngoài ca |
| S02 | Nhân viên căn tin | Phục vụ đúng ca | Món ăn, trò chuyện ngắn | Bữa ăn cần menu và giá đã author |
| S03 | Tài xế xe buýt | Chạy theo tuyến | Lên xe, hỏi tuyến | Không đổi lịch để chờ player |
| S04 | Bạn lớp | Hoàn thành bài nhóm | Học cùng, chia việc | Không mặc định tin tưởng player |
| S05 | Sinh viên mới | Tìm đường | Player giúp hoặc từ chối | Chỉ khi location và thời gian phù hợp |
| S06 | Trưởng CLB | Tuyển và phân việc | Giới thiệu CLB | Từ chối không khóa nghề |
| S07 | Người tuyển dụng | Tìm ứng viên | JD, screening | Không đảm bảo offer |
| S08 | Đồng nghiệp thực tập | Hoàn thành công việc | Review, hỗ trợ | Chỉ khi có internship phù hợp |

Mỗi secondary cần hồ sơ chi tiết và sự kiện riêng trước production; không coi bảng này là 8 arc hoàn chỉnh.

## Gate QA chưa đạt

Chưa có lịch theo tuần, predicate, số thay đổi, thoại, priority, tác động hoặc alternate cho 63 scene seeds; chưa chứng minh 12–20 scene/người có thể trải qua trong lịch hoặc kết nối callback. Cần kiểm thử chưa gặp, từ chối lời mời, NPC bận, trust cao + conflict mở, NPC không biết thông tin và năm 5/6 không mặc định sự nghiệp của Long. Hiện không chấm điểm chất lượng hay xác nhận đã hoàn thiện.
