# Gameplay systems plan — Phòng 404

Tài liệu này quy định vòng chơi và vai trò của các hệ thống. Thời điểm/cầu nối giữa mission thuộc [Gameplay transitions plan](GAMEPLAY_TRANSITIONS_PLAN.md); các mốc canon thuộc [Production flow baseline](PRODUCTION_FLOW_BASELINE.md).

## 1. Core loop

```text
Người chơi muốn làm gì?
        ↓
Chọn cách dùng thời gian
        ↓
Hoạt động có feedback
        ↓
Hệ quả lên học tập, tiền hoặc quan hệ
        ↓
Chấp nhận hoặc phục hồi với chi phí
        ↓
State đổi → bạn bè/story phản ứng
        ↓
Vấn đề hoặc milestone mới
```

## 2. Loại mission

- **Signature gameplay:** hoạt động người chơi nhớ được; mục tiêu ban đầu khoảng 10–15 trong 45 mission sau khi audit toàn tuyến.
- **Decision scene:** lựa chọn là gameplay chính; phải có trade-off hoặc phản ứng quan sát được.
- **Exploration/interaction:** đi lại, xem đồ, tương tác ngắn hoặc nói chuyện.
- **Narrative transition:** ngủ, montage, kết quả học kỳ hoặc thay đổi căn phòng; không gắn minigame giả tạo.

Đánh giá variety theo động từ của người chơi: chọn/sắp lịch, tìm/sửa, quan sát/lén, ưu tiên, chẩn đoán, phản hồi, giải bài, giải thích/phản biện. Nhiều mission liền nhau chỉ yêu cầu bấm nhanh/giữ nút là dấu hiệu cần gộp hoặc thiết kế lại.

## 3. State và resource

### Luôn có

- **Time:** giới hạn cơ hội; thời gian bị dùng phải hiện trong lịch hoặc transition summary.
- **Academic:** môn, bài, điểm danh, tín chỉ và tiên quyết theo canon. UI chỉ hiện thông tin cần cho quyết định kế tiếp.
- **Relationship:** ưu tiên thể hiện qua hành vi, lời thoại, availability và trợ giúp của từng NPC; không cần thanh Bromance làm trung tâm.

### Chỉ mở khi tạo lựa chọn

- **Energy:** dùng cho thức khuya, học và lịch sáng; không biến thành stamina phải nạp liên tục.
- **Money:** dùng khi thiếu tiền buộc chọn làm thêm, đóng học phí, đi lại hoặc hoạt động khác. Job thường không lặp vô hạn để farm.

## 4. Consequence và recovery

- Báo hiệu rủi ro trước lựa chọn khi nhân vật có thể biết rủi ro đó.
- Kết quả cho biết chuyện gì vừa xảy ra và nguyên nhân liên quan đến lựa chọn nào.
- Recovery cho biết trước thời gian/tiền/hoạt động phải hy sinh.
- Một sai lầm nhỏ không khóa campaign; tránh chuỗi phạt tự lặp khiến hết lựa chọn khả thi.
- Canon academic outcome chỉ đổi khi baseline được cập nhật. Nếu outcome cố định, gameplay phải nói rõ lựa chọn ảnh hưởng tới đường đi/chi phí nào để không giả vờ người chơi có thể đổi kết quả.

## 5. Relationship và phòng 404

Relationship được ghi nhận qua những sự kiện đáng nhớ và tier phản ứng, không phải một điểm duy nhất áp cho mọi người. Tách sự kiện đã xảy ra (ví dụ đã giúp ôn thi) khỏi thái độ lâu dài để các trạng thái không mâu thuẫn.

Phòng 404 thay đổi bằng vật dụng, giường trống, lịch sinh hoạt và người mới. Sau các biến cố lớn, cho người chơi trở lại phòng để thấy thay đổi trước khi time-skip tiếp theo.

## 6. Rule kiểm tra mỗi activity

Trước khi duyệt activity, trả lời:

1. Người chơi tự tay làm hoặc quyết định điều gì?
2. Có kỹ năng, áp lực hoặc lựa chọn nào khiến tương tác đáng chơi?
3. Feedback cho biết kết quả ra sao?
4. Activity thay đổi state, quan hệ hoặc hiểu biết của người chơi thế nào?

Nếu chỉ có giao diện khác mà động từ/thử thách vẫn giống activity trước, rút ngắn hoặc thiết kế lại.

## 7. Vai trò hệ thống trong sáu năm — canon 26/10/2026

- Năm 1 làm quen; Năm 2 trả giá; Năm 3 repair kiếm sống và cứu việc học; Năm 4 học/tiền/môn nợ/đồ án nhỏ; Năm 5 tải cao nhất với học nợ, chuẩn bị đồ án, internship có điều kiện, repair, gia đình và chia tay; Năm 6 giải quyết và payoff.
- Năm 5 internship chỉ có task đại diện đủ requirement/clarification/code/test/review/feedback, có lịch và eligibility. Không tự tạo company/NPC/offer/thu nhập; không payroll/production incident/promotion/job hopping thành loop chính. Công việc sau tốt nghiệp chỉ là epilogue sau credits.
- CTDL PASS mới mở HĐH; lịch mới đặt HĐH môn cuối ở đầu Năm 6, M36 xét đủ điều kiện rồi M37 đăng ký đồ án. Chuẩn bị/research/prototype từ Năm 5 được thẩm định lại, không auto hoàn thành đồ án. Không ép học lại môn đã PASS, không xóa kết quả để kéo dài năm.
- Năm 6 reuse academic/time/repair/ledger/project/social/room. Ca repair cuối chỉ mở khi có ca tồn/khách và điều kiện được author, không tự spawn phần thưởng cứu học phí. Bảo vệ đạt khác với đủ điều kiện nhận bằng; học phí còn thực nợ được đối soát/đóng theo ledger trước lễ, không invent khoản nợ.
- Dọn phòng là gameplay tháo đồ của mình/gấp chăn/đặt laptop và dụng cụ vào balo/nhìn lại/trả khóa. Giữ người mới và đồ của họ; “phòng trống” là góc Lươn hoặc hồi tưởng. Quan hệ và flashback lấy từ history; không một final choice ghi đè sáu năm.
- Chỉ mở lễ/ending khi đủ conditions thật. Thiếu điểm, tiền hoặc đồ án chưa đạt cần recovery có thời gian/cost; không ép A/PASS hoặc Finished sớm. Save cũ khác canon cần mapping/migration bảo toàn bản gốc trước triển khai.
