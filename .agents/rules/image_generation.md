# Image & Sprite Generation Rules

Khi có yêu cầu hoặc hành động cần tạo ảnh/asset mỹ thuật (sprites, VFX, icons, UI textures, backgrounds, v.v.):

Trước hết kiểm tra asset có sẵn. Thay đổi bố cục, vị trí, tỷ lệ, pivot hoặc mirroring không cần generate ảnh mới nếu asset hiện có đã đáp ứng yêu cầu. Không tạo lại ảnh chỉ để làm thành batch.

1. **Tuyệt đối không tạo lẻ từng ảnh đơn lẻ**:
   - Không gọi tool tạo ảnh (Image Generation) cho từng asset riêng biệt nếu các asset đó có thể gom nhóm được.

2. **Gom nhiều asset theo batch (Sprite Sheet / Grid Canvas)**:
   - Trước khi gọi tool, lập danh sách asset/frame cần tạo trong yêu cầu hiện tại; ghi số hàng, số cột, kích thước cell và thứ tự đọc từ trái sang phải, từ trên xuống dưới.
   - Luôn gom nhiều frame hoạt ảnh (animation frames), biến thể tư thế, hướng di chuyển, hoặc tập hợp icons/props/VFX liên quan vào trong **cùng một canvas duy nhất** (ví dụ grid 2x2, 4x2, 4x1, v.v.).
   - Định rõ layout grid, khoảng cách lề (margins), và màu nền đồng nhất (pure `#FFFFFF` hoặc flat `#000000`) để dễ dàng tách ảnh.

3. **Cắt nhỏ (Slice / Extract) thành sub-images sau khi sinh ảnh**:
   - Sử dụng script (Python/Pillow hoặc Unity sprite slicing tool) để cắt canvas thành các file sub-images/sprites riêng biệt.
   - Lưu trữ prompt và metadata sinh ảnh vào tài liệu (như `assets/sprites/GENERATION.md`).
   - Giữ sheet nguồn trong workspace; ghi kích thước canvas, grid, lề, tọa độ crop, tên file đầu ra, pivot và pixels-per-unit để có thể slice lại đúng như cũ.
   - Kiểm tra không có sprite/frame bị cắt cụt hoặc lẫn sang cell bên cạnh trước khi tích hợp.
   - Phân biệt rõ ảnh generate theo batch, ảnh đơn từ trước và sheet chỉ đóng gói lại từ asset có sẵn. Việc ghép các ảnh đơn cũ thành một sheet không biến chúng thành ảnh đã generate theo batch.

4. **Mục tiêu**:
   - **Tối ưu hóa số lượng token** và lượt gọi tool tạo ảnh.
   - Đảm bảo tính nhất quán cao về phong cách mỹ thuật (art style), tỷ lệ và bảng màu giữa các asset.
