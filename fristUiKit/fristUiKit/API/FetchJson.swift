//
//  FetchJson.swift
//  fristSwiftUI
//
//  Created by 尤凱 on 2025/4/10.
//

import Foundation

public func fetchJSON(from urlString: String, completion: @escaping (Result<[String: Any], Error>) -> Void) {
    guard let url = URL(string: urlString) else {
        completion(.failure(NSError(domain: "Invalid URL", code: 0, userInfo: nil)))
        return
    }

    let task = URLSession.shared.dataTask(with: url) { (data, response, error) in
        if let error = error {
            completion(.failure(error))
            return
        }

        guard let httpResponse = response as? HTTPURLResponse,
              (200...299).contains(httpResponse.statusCode) else {
            completion(.failure(NSError(domain: "Invalid HTTP Status Code", code: 0, userInfo: nil)))
            return
        }

        guard let data = data else {
            completion(.failure(NSError(domain: "No Data Received", code: 0, userInfo: nil)))
            return
        }

        do {
            // 將 Data 轉換為 String，以便進行 Unicode 解碼
            if let responseString = String(data: data, encoding: .utf8) {
                // 使用 JSONSerialization 解析 String
                if let json = try JSONSerialization.jsonObject(with: responseString.data(using: .utf8)!, options: []) as? [String: Any] {
                    // 在主執行緒回傳解析後的 JSON
                    DispatchQueue.main.async {
                        completion(.success(json))
                    }
                } else {
                    DispatchQueue.main.async {
                        completion(.failure(NSError(domain: "Invalid JSON Format", code: 0, userInfo: nil)))
                    }
                }
            } else {
                DispatchQueue.main.async {
                    completion(.failure(NSError(domain: "Failed to decode response string", code: 0, userInfo: nil)))
                }
            }
        } catch {
            DispatchQueue.main.async {
                completion(.failure(error))
            }
        }
    }

    task.resume() // 開始執行網路請求
}
