import Foundation

protocol APIResult {
    func getData() -> [String: Any]?
}

// 實作 APIResult protocol
struct SwiftAPIResult: APIResult {
    var data: [String: Any]?

    func getData() -> [String: Any]? {
        return data
    }
}
