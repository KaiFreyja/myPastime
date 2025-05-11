//
//  postJSON.swift
//  fristUiKit
//
//  Created by 尤凱 on 2025/5/8.
//

import Foundation

public func postJSON(to urlString: String, body: [String: Any], completion: @escaping (Result<[String: Any], Error>) -> Void) {
    guard let url = URL(string: urlString) else {
        completion(.failure(NSError(domain: "Invalid URL", code: 0, userInfo: nil)))
        return
    }

    // 轉換字典為 JSON 資料
    guard let jsonData = try? JSONSerialization.data(withJSONObject: body, options: []) else {
        completion(.failure(NSError(domain: "Invalid JSON Body", code: 0, userInfo: nil)))
        return
    }

    var request = URLRequest(url: url)
    request.httpMethod = "POST"
    request.setValue("application/json", forHTTPHeaderField: "Content-Type")
    request.httpBody = jsonData

    let task = URLSession.shared.dataTask(with: request) { (data, response, error) in
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
            // 嘗試解析 JSON 回應
            if let json = try JSONSerialization.jsonObject(with: data, options: []) as? [String: Any] {
                // 在主執行緒回傳解析後的 JSON
                DispatchQueue.main.async {
                    completion(.success(json))
                }
            } else {
                DispatchQueue.main.async {
                    completion(.failure(NSError(domain: "Invalid JSON Format", code: 0, userInfo: nil)))
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
