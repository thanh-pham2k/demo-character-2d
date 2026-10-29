# Image & Sprite Generation Rules

Khi có yêu cầu hoặc hành động cần tạo ảnh/asset mỹ thuật (sprites, VFX, icons, UI textures, backgrounds, v.v.):

Trước hết kiểm tra asset có sẵn. Thay đổi bố cục, vị trí, tỷ lệ, pivot hoặc mirroring không cần generate ảnh mới nếu asset hiện có đã đáp ứng yêu cầu. Không tạo lại ảnh chỉ để làm thành batch.

1. **Tuyệt đối không tạo lẻ từng ảnh đơn lẻ**:
   - Không gọi tool tạo ảnh (Image Generation) cho từng asset riêng biệt nếu các asset đó có thể gom nhóm được.

2. **Gom nhiều asset theo batch (Sprite Sheet / Grid Canvas)**:
   - Trước khi gọi tool, lập danh sách asset/frame cần tạo trong yêu cầu hiện tại; ghi số hàng, số cột, kích thước cell và thứ tự đọc từ trái sang phải, từ trên xuống dưới.
   - Luôn gom nhiều frame hoạt ảnh (animation frames), biến thể tư thế, hướng di chuyển, hoặc tập hợp icons/props/VFX liên quan vào trong **cùng một canvas duy nhất** (ví dụ grid 2x2, 4x2, 4x1, v.v.).
   - Định rõ layout grid, khoảng cách lề (margins), và nền trong suốt hoặc màu nền đồng nhất (pure `#FFFFFF` hoặc flat `#000000`) phù hợp để tách ảnh.
   - Trước khi tạo, xác định kích thước hiển thị tối đa theo nền tảng, tỉ lệ khung hình, kích thước pixel mỗi cell/frame, khoảng trống và dung lượng mục tiêu. Chọn canvas đủ chứa toàn bộ asset, không cắt mép hoặc làm lẫn cell; các frame cùng animation cần cùng kích thước và pivot.

3. **Cắt nhỏ (Slice / Extract) thành sub-images sau khi sinh ảnh**:
   - Sử dụng script (Python/Pillow hoặc Unity sprite slicing tool) để cắt canvas thành các file sub-images/sprites riêng biệt.
   - Lưu trữ prompt và metadata sinh ảnh vào tài liệu (như `assets/sprites/GENERATION.md`).
   - Giữ sheet nguồn trong workspace; ghi kích thước canvas, grid, lề, tọa độ crop, tên file đầu ra, pivot và pixels-per-unit để có thể slice lại đúng như cũ.
   - Kiểm tra không có sprite/frame bị cắt cụt hoặc lẫn sang cell bên cạnh trước khi tích hợp.
   - Phân biệt rõ ảnh generate theo batch, ảnh đơn từ trước và sheet chỉ đóng gói lại từ asset có sẵn. Việc ghép các ảnh đơn cũ thành một sheet không biến chúng thành ảnh đã generate theo batch.

4. **Định dạng WebP và kích thước nhẹ cho web**:
   - Ưu tiên WebP cho ảnh raster dùng tại runtime web; bảo toàn alpha với sprite trong suốt. Chọn lossy hoặc lossless theo nội dung, so sánh dung lượng và giữ chi tiết, màu sắc, cạnh cùng độ trong suốt chấp nhận được.
   - Sau slicing, xuất từng frame hoặc atlas ở kích thước vừa đủ cho màn hình lớn nhất dự kiến, tính cả mật độ điểm ảnh khi cần. Không phóng đại nguồn chất lượng thấp, không lưu ảnh lớn hơn nhu cầu hoặc nhiều bản trùng lặp không có mục đích.
   - Ghi vào metadata kích thước sheet và từng frame, kích thước hiển thị mục tiêu, định dạng/nén, dung lượng và phiên bản nguồn; kiểm tra kích thước, dung lượng, alpha và mép cắt trước khi tích hợp.
   - Giữ sheet nguồn để tái tạo và giữ SVG/vector gốc khi cần chỉnh sửa; WebP là bản runtime raster, không thay thế nguồn có khả năng chỉnh sửa. Không đổi định dạng asset đang dùng nếu không cần cho yêu cầu.

5. **Mục tiêu**:
   - **Tối ưu hóa số lượng token** và lượt gọi tool tạo ảnh.
   - Đảm bảo tính nhất quán cao về phong cách mỹ thuật (art style), tỷ lệ và bảng màu giữa các asset.
