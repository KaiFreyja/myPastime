<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use GuzzleHttp\Client;

class GeminiController extends Controller
{
    protected $client;
    protected $apiKey;

    /**
     * Create a new controller instance.
     *
     * @return void
     */
    public function __construct()
    {
        //
        $this->client = new Client();
        $this->apiKey = env('GEMINI_API_KEY');
    }

    public function ask()
    {
        $client = new Client();
        $apiKey = env('GEMINI_API_KEY');
        $prompt = request()->get('q', '請介紹一下台灣');

        $url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={$apiKey}";

        $body = [
            'contents' => [
                [
                    'parts' => [
                        ['text' => "請顯示中文\n".$prompt]
                    ]
                ]
            ]
        ];

        $response = $client->post($url, [
            'json' => $body,
        ]);

        $data = json_decode($response->getBody(), true);
        return $data['candidates'][0]['content']['parts'][0]['text'] ?? '[無回應]';
    }


    public function formalAsk($input)
    {
        $client = new Client();
        $apiKey = env('GEMINI_API_KEY');
        $url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={$apiKey}";

        $contents = [];
        foreach($input as $item)
        {
            $role = $item->talker === 'user'?'user':'model';
            $text = $item->content ?? '';

            $contents[] = [
                'role' => $role,
                'parts' => [['text' => $text]],
            ];
        }

        $body = ['contents' => $contents];

        $response = $client->post($url, [
            'json' => $body,
        ]);

        $data = json_decode($response->getBody(), true);
        return $data['candidates'][0]['content']['parts'][0]['text'] ?? '[無回應]';
    }
}
