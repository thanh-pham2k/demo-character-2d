# Codebase Rules & Guidelines

## 1. Asset & Image Generation (Batching & Token Optimization)
- **Không sinh lẻ từng ảnh**: Khi cần tạo ảnh/assets (sprites, VFX, UI icons, nhân vật), không thực hiện generate từng asset đơn lẻ.
- **Tạo theo batch (Grid / Sheet)**: Gom nhiều assets, các frame animation hoặc biến thể vào cùng 1 canvas duy nhất theo dạng grid (ví dụ 4x2, 4x1, 2x2...).
- **Cắt ra sub-images**: Sau khi generate ảnh tổng hợp, sử dụng script cắt (slicing) để xuất thành các sub-images/frames riêng biệt.
- **Tối ưu chi phí & Token**: Giảm thiểu số lượt gọi tool tạo ảnh và duy trì phong cách đồ họa (art style/palette) đồng nhất giữa các asset.

- **Ưu tiên tái sử dụng**: Với thay đổi vị trí, tỷ lệ, pivot hoặc bố cục, dùng asset có sẵn khi đáp ứng yêu cầu; không generate lại chỉ để tạo thêm batch.
- **Lập kế hoạch trước khi generate**: Liệt kê asset/frame cần tạo, chọn layout grid và cách slicing trước khi gọi tool. Ảnh đơn đã có phải được ghi đúng là ảnh đơn, không đổi tên thành batch.

Xem chi tiết tại: [.agents/rules/image_generation.md](.agents/rules/image_generation.md)

## 2. Fast Implementation & Manual Testing

Quy định này thay thế yêu cầu tự chạy test/lint/compile trước đây. Người dùng tự kiểm tra gameplay bằng checkpoint.

- Ưu tiên hoàn thành code; thay đổi tối thiểu, bám architecture hiện có.
- Không tự chạy test suite, lint, compile, Unity Play Mode, batch mode hoặc build; chỉ chạy khi người dùng yêu cầu.
- Không tạo bản sao project chỉ để kiểm tra.
- Chỉ kiểm tra diff và tham chiếu code liên quan.
- Nếu Unity đang mở và có lỗi compile, sửa lỗi do thay đổi của mình; không chủ động khởi chạy Unity để kiểm tra.

### Checkpoint bắt buộc

- Khi thêm hoặc sửa flow gameplay, cung cấp checkpoint có thể chọn trong F8 để tải đúng tiến độ, vị trí, pose, UI, thời gian và điều kiện liên quan.
- Checkpoint dùng cơ chế state/restore hiện có; reset phiên trước để tránh coroutine, reservation, UI hoặc trạng thái cũ còn sót.
- Dữ liệu checkpoint kiểm tra phải tách khỏi save chơi chính.
- Ghi rõ: **checkpoint → thao tác → kết quả mong đợi**.
- Người dùng tự kiểm tra gameplay, UI và animation.

## 3. Agent Fast Rules

- **Execute first. Explain only if asked.**
- Không lặp lại yêu cầu hoặc mô tả kế hoạch.
- Không hỏi nếu có thể tự suy luận hợp lý.
- Đọc code trước, bám architecture/convention hiện tại.
- Minimal change; không sửa phần không liên quan.
- Không invent API, field, function hoặc business rule.
- Tái sử dụng code hiện có.
- Tuân thủ quy định kiểm tra ở mục 2; không tự chạy test/lint/compile.
- Nếu gặp blocker, tự thử phương án khác trước.
- Output cuối bắt đầu bằng **TLDR** ngắn gọn, sau đó chỉ gồm thông tin cần thiết: **đã làm gì, file đã sửa, checkpoint, blocker nếu có**; không lặp lại nội dung TLDR.
- Ghi rõ phần chưa kiểm tra; không tuyên bố đã hoạt động đúng chỉ dựa trên việc đọc code.

## 4. Phong cách trả lời — The Elements of Style

- Luôn mở đầu câu trả lời cuối bằng **TLDR** ngắn gọn (1–2 câu), nêu ngay kết quả hoặc ý chính. Với việc đơn giản, TLDR có thể là toàn bộ câu trả lời nếu đã đủ thông tin cần thiết.

- Trả lời ngắn gọn, rõ ràng, trực tiếp; bỏ từ và câu không cần thiết.
- Ưu tiên câu chủ động, từ cụ thể và cách diễn đạt đơn giản.
- Nêu kết quả trước; chỉ giải thích thêm khi cần hoặc được hỏi.
- Không lặp ý, sáo ngữ, mở đầu xã giao hoặc kết luận thừa.
- Giữ đủ thông tin thiết yếu; không đánh đổi tính chính xác để rút ngắn.
- Tuân thủ định dạng output cuối trong Agent Fast Rules.
