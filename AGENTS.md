# Codebase Rules & Guidelines

## 1. Asset & Image Generation (Batching & Token Optimization)
- **Không sinh lẻ từng ảnh**: Khi cần tạo ảnh/assets (sprites, VFX, UI icons, nhân vật), không thực hiện generate từng asset đơn lẻ.
- **Tạo theo batch (Grid / Sheet)**: Gom nhiều assets, các frame animation hoặc biến thể vào cùng 1 canvas duy nhất theo dạng grid (ví dụ 4x2, 4x1, 2x2...).
- **Cắt ra sub-images**: Sau khi generate ảnh tổng hợp, sử dụng script cắt (slicing) để xuất thành các sub-images/frames riêng biệt.
- **Tối ưu chi phí & Token**: Giảm thiểu số lượt gọi tool tạo ảnh và duy trì phong cách đồ họa (art style/palette) đồng nhất giữa các asset.

- **Ưu tiên tái sử dụng**: Với thay đổi vị trí, tỷ lệ, pivot hoặc bố cục, dùng asset có sẵn khi đáp ứng yêu cầu; không generate lại chỉ để tạo thêm batch.
- **Lập kế hoạch trước khi generate**: Liệt kê asset/frame cần tạo, chọn layout grid và cách slicing trước khi gọi tool. Ảnh đơn đã có phải được ghi đúng là ảnh đơn, không đổi tên thành batch.

Xem chi tiết tại: [.agents/rules/image_generation.md](.agents/rules/image_generation.md)
