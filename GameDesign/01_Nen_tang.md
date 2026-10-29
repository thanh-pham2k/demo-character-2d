# Nền tảng thiết kế — bản nháp

## Fantasy và pillars

Người chơi bắt đầu ở KTX xa lạ, tự xây nhịp sống đại học và bước vào công việc đầu tiên với một lịch sử riêng. Bốn pillar: thời gian có giá; người khác có đời sống riêng; học tập mở năng lực thực tế; thất bại tạo đường hồi phục và câu chuyện khác.

## Đơn vị quyết định

Một lượt là người chơi chọn một hoạt động có thời lượng và điều kiện rõ, hoặc phản hồi một sự kiện. Trước xác nhận, UI cho biết thời điểm dự kiến kết thúc, chi phí chắc chắn và các lịch đã biết có nguy cơ bị lỡ; kết quả chưa chắc chắn phải được ghi là chưa chắc chắn. Người chơi có thể xem lịch, lựa chọn khác hoặc hủy mà không đổi state.

Sau xác nhận, hoạt động tiêu thụ thời gian, áp dụng tác động và xử lý sự kiện có thời điểm nằm trong khoảng đó theo thứ tự thời gian. Khi sự kiện yêu cầu lựa chọn, đồng hồ dừng, thông tin mới được hiển thị, người chơi có thể tiếp tục, thay kế hoạch hoặc bỏ qua nếu sự kiện cho phép. Không dùng một con số uy tín duy nhất làm thay mọi quan hệ.

## Vòng chơi

Lịch và nhu cầu → chọn ưu tiên → hoạt động tiêu thụ thời gian → thông tin/sự kiện thay kế hoạch → lựa chọn → hậu quả trực tiếp → lưu trạng thái/quan hệ/kiến thức → callback tương lai → recap. Một ngày không bắt buộc có cả nhiệm vụ chính lẫn sự kiện phụ; nội dung tùy lịch và trạng thái.

## Lát cắt kiểm chứng giả thuyết

Ngày 2, trước lớp, Long rủ chơi. Player thấy giờ hiện tại, giờ xe và giờ học. Chơi tăng cơ hội tương tác và tiêu tốn thời gian; đi học giữ thời gian di chuyển. Cảnh báo theo các mốc đã được thiết kế; đến muộn được tiếp tục học trong thời gian còn lại. Buổi tối là một lựa chọn khác: học hoặc chơi với Long. Topic yếu và lịch sử với Long tạo phản hồi ở kỳ ôn; sự kiện phải có phương án không chơi, lỡ xe và học bù. Các lượng cộng/trừ, thời gian xe và phản ứng NPC chưa cân bằng hoặc author đầy đủ.

## Quyết định chưa chốt

Quy mô ngày có thể chơi; đơn vị thời gian, tốc độ đồng hồ, giá trị tiền/năng lượng; chính sách lớp chưa tham gia; công thức topic và thi; lượng nội dung NPC; điều kiện tốt nghiệp, nhánh năm 5–6, offer nghề; dữ liệu save và migration; event arbitration; kinh phí production. Không giải quyết bằng số giả để tự tuyên bố đạt DoD.
